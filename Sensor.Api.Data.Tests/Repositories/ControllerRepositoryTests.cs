using System.Data;
using System.Data.Common;
using System.Collections;
using Moq;
using Sensor.Api.Data.QueryResults;
using Sensor.Api.Data.Repositories;
using Xunit;

namespace Sensor.Api.Data.Tests.Repositories;

public class ControllerRepositoryTests
{
    [Fact]
    public async Task GetAllControllersAsync_ReturnsMappedControllerRows()
    {
        var expectedRows = new[]
        {
            new Dictionary<string, object?>
            {
                ["Id"] = 1,
                ["ControllerKey"] = "ctrl-01",
                ["Name"] = "First Controller",
                ["LocationId"] = 12,
                ["LocationName"] = "Warehouse",
                ["IsActive"] = true,
                ["CreatedUtc"] = new DateTime(2026, 6, 11, 8, 0, 0, DateTimeKind.Utc),
                ["SensorCount"] = 3
            }
        };

        var connection = new FakeDbConnection(FakeCommandResult.Reader(expectedRows));
        var dbContext = new Mock<IDbContext>();
        dbContext.Setup(x => x.CreateConnection()).Returns(connection);

        var repository = new ControllerRepository(dbContext.Object);
        var controllers = await repository.GetAllControllersAsync();

        Assert.Single(controllers);
        var controller = Assert.Single(controllers);
        Assert.Equal(1, controller.Id);
        Assert.Equal("ctrl-01", controller.ControllerKey);
        Assert.Equal("First Controller", controller.Name);
        Assert.Equal(12, controller.LocationId);
        Assert.Equal("Warehouse", controller.LocationName);
        Assert.True(controller.IsActive);
        Assert.Equal(3, controller.SensorCount);
    }

    [Fact]
    public async Task GetControllerByIdAsync_ReturnsControllerWhenFound()
    {
        var expectedRows = new[]
        {
            new Dictionary<string, object?>
            {
                ["Id"] = 7,
                ["ControllerKey"] = "ctrl-07",
                ["Name"] = "Controller Seven",
                ["LocationId"] = 4,
                ["LocationName"] = "Factory",
                ["IsActive"] = false,
                ["CreatedUtc"] = new DateTime(2024, 1, 1, 10, 30, 0, DateTimeKind.Utc),
                ["SensorCount"] = 0
            }
        };

        var connection = new FakeDbConnection(FakeCommandResult.Reader(expectedRows));
        var dbContext = new Mock<IDbContext>();
        dbContext.Setup(x => x.CreateConnection()).Returns(connection);

        var repository = new ControllerRepository(dbContext.Object);
        var controller = await repository.GetControllerByIdAsync(7);

        Assert.NotNull(controller);
        Assert.Equal(7, controller!.Id);
        Assert.Equal("ctrl-07", controller.ControllerKey);
        Assert.False(controller.IsActive);
        Assert.Equal(0, controller.SensorCount);
    }

    [Fact]
    public async Task GetControllerByIdAsync_ReturnsNullWhenNotFound()
    {
        var connection = new FakeDbConnection(FakeCommandResult.Reader(Array.Empty<Dictionary<string, object?>>()));
        var dbContext = new Mock<IDbContext>();
        dbContext.Setup(x => x.CreateConnection()).Returns(connection);

        var repository = new ControllerRepository(dbContext.Object);
        var controller = await repository.GetControllerByIdAsync(42);

        Assert.Null(controller);
    }

    [Fact]
    public async Task CreateAsync_ReturnsGeneratedId()
    {
        var connection = new FakeDbConnection(FakeCommandResult.Scalar(99));
        var dbContext = new Mock<IDbContext>();
        dbContext.Setup(x => x.CreateConnection()).Returns(connection);

        var repository = new ControllerRepository(dbContext.Object);
        var request = new CreateControllerQR
        {
            ControllerKey = "ctrl-new",
            Name = "New Controller",
            LocationId = 5
        };

        var id = await repository.CreateAsync(request);

        Assert.Equal(99, id);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsTrueWhenRowsAffected()
    {
        var connection = new FakeDbConnection(FakeCommandResult.AffectedRows(1));
        var dbContext = new Mock<IDbContext>();
        dbContext.Setup(x => x.CreateConnection()).Returns(connection);

        var repository = new ControllerRepository(dbContext.Object);
        var request = new UpdateControllerQR
        {
            ControllerKey = "ctrl-updated",
            Name = "Updated Controller",
            LocationId = 2,
            IsActive = true
        };

        var success = await repository.UpdateAsync(8, request);

        Assert.True(success);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalseWhenNoRowsAffected()
    {
        var connection = new FakeDbConnection(FakeCommandResult.AffectedRows(0));
        var dbContext = new Mock<IDbContext>();
        dbContext.Setup(x => x.CreateConnection()).Returns(connection);

        var repository = new ControllerRepository(dbContext.Object);
        var request = new UpdateControllerQR
        {
            ControllerKey = "ctrl-none",
            Name = "No Update",
            LocationId = 2,
            IsActive = false
        };

        var success = await repository.UpdateAsync(999, request);

        Assert.False(success);
    }

    [Fact]
    public async Task GetControllerKey_ReturnsNextSequenceNumber()
    {
        var connection = new FakeDbConnection(FakeCommandResult.Scalar(5));
        var dbContext = new Mock<IDbContext>();
        dbContext.Setup(x => x.CreateConnection()).Returns(connection);

        var repository = new ControllerRepository(dbContext.Object);
        var nextSequence = await repository.GetControllerKey(10);

        Assert.Equal(5, nextSequence);
    }

    [Fact]
    public async Task GetNextControllerKeySequenceNumberAsync_ReturnsSequenceNumber()
    {
        var connection = new FakeDbConnection(FakeCommandResult.Scalar(12));
        var dbContext = new Mock<IDbContext>();
        dbContext.Setup(x => x.CreateConnection()).Returns(connection);

        var repository = new ControllerRepository(dbContext.Object);
        var nextSequence = await repository.GetNextControllerKeySequenceNumberAsync(15);

        Assert.Equal(12, nextSequence);
    }
}

internal sealed class FakeDbConnection : DbConnection
{
    private readonly FakeCommandResult _result;
    private ConnectionState _state;

    public FakeDbConnection(FakeCommandResult result)
    {
        _result = result;
    }

    public override string ConnectionString { get; set; } = string.Empty;
    public override string Database => "FakeDatabase";
    public override string DataSource => "FakeDataSource";
    public override string ServerVersion => "1.0";
    public override ConnectionState State => _state;

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => throw new NotSupportedException();
    public override void ChangeDatabase(string databaseName) => throw new NotSupportedException();
    public override void Close() => _state = ConnectionState.Closed;
    public override void Open() => _state = ConnectionState.Open;
    public override Task OpenAsync(CancellationToken cancellationToken) { _state = ConnectionState.Open; return Task.CompletedTask; }
    protected override DbCommand CreateDbCommand() => new FakeDbCommand(_result) { Connection = this };
}

internal sealed class FakeDbCommand : DbCommand
{
    private readonly FakeCommandResult _result;
    private readonly FakeDbParameterCollection _parameters = new();
    protected override DbParameterCollection DbParameterCollection => _parameters;

    public FakeDbCommand(FakeCommandResult result)
    {
        _result = result;
    }

    public override string CommandText { get; set; } = string.Empty;
    public override int CommandTimeout { get; set; }
    public override CommandType CommandType { get; set; }
    public override bool DesignTimeVisible { get; set; }
    public override UpdateRowSource UpdatedRowSource { get; set; }
    protected override DbConnection? DbConnection { get; set; }
    protected override DbTransaction? DbTransaction { get; set; }

    protected override DbParameter CreateDbParameter() => new FakeDbParameter();
    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        if (_result.Rows != null)
        {
            return new FakeDbDataReader(_result.Rows);
        }

        return new FakeDbDataReader(Array.Empty<Dictionary<string, object?>>());
    }

    public override int ExecuteNonQuery() => _result.RowsAffected;
    public override object? ExecuteScalar() => _result.ScalarResult;
    public override void Cancel() { }
    public override void Prepare() { }
    protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken) => Task.FromResult<DbDataReader>(ExecuteDbDataReader(behavior));
    public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken) => Task.FromResult(ExecuteNonQuery());
    public override Task<object?> ExecuteScalarAsync(CancellationToken cancellationToken) => Task.FromResult(ExecuteScalar());
}

internal sealed class FakeDbParameter : DbParameter
{
    public override DbType DbType { get; set; }
    public override ParameterDirection Direction { get; set; } = ParameterDirection.Input;
    public override bool IsNullable { get; set; }
    public override string ParameterName { get; set; } = string.Empty;
    public override string SourceColumn { get; set; } = string.Empty;
    public override object? Value { get; set; }
    public override bool SourceColumnNullMapping { get; set; }
    public override int Size { get; set; }
    public override void ResetDbType() { }
}

internal sealed class FakeDbParameterCollection : DbParameterCollection
{
    private readonly List<DbParameter> _parameters = new();
    public override int Count => _parameters.Count;
    public override object SyncRoot => new object();
    public override int Add(object? value)
    {
        if (value is DbParameter parameter)
        {
            _parameters.Add(parameter);
            return _parameters.Count - 1;
        }

        throw new ArgumentException("Value must be a DbParameter.");
    }

    public override void AddRange(Array values)
    {
        foreach (var value in values)
        {
            Add(value);
        }
    }

    public override void Clear() => _parameters.Clear();
    public override bool Contains(object? value) => _parameters.Contains(value as DbParameter);
    public override bool Contains(string parameterName) => _parameters.Any(p => p.ParameterName == parameterName);
    public override void CopyTo(Array array, int index) => _parameters.ToArray().CopyTo(array, index);
    public override IEnumerator GetEnumerator() => ((IEnumerable)_parameters).GetEnumerator();
    public override int IndexOf(object? value) => _parameters.IndexOf(value as DbParameter);
    public override int IndexOf(string parameterName) => _parameters.FindIndex(p => p.ParameterName == parameterName);
    public override void Insert(int index, object? value) => _parameters.Insert(index, (DbParameter)value!);
    public override void Remove(object? value) => _parameters.Remove(value as DbParameter);
    public override void RemoveAt(int index) => _parameters.RemoveAt(index);
    public override void RemoveAt(string parameterName)
    {
        var index = IndexOf(parameterName);
        if (index >= 0)
        {
            RemoveAt(index);
        }
    }

    protected override DbParameter GetParameter(int index) => _parameters[index];
    protected override DbParameter GetParameter(string parameterName) => _parameters.First(p => p.ParameterName == parameterName);
    protected override void SetParameter(int index, DbParameter value) => _parameters[index] = value;
    protected override void SetParameter(string parameterName, DbParameter value)
    {
        var index = IndexOf(parameterName);
        if (index == -1)
        {
            Add(value);
        }
        else
        {
            _parameters[index] = value;
        }
    }
}

internal sealed class FakeDbDataReader : DbDataReader
{
    private readonly IReadOnlyList<Dictionary<string, object?>> _rows;
    private int _rowIndex = -1;
    private readonly List<string> _columns;

    public FakeDbDataReader(IReadOnlyList<Dictionary<string, object?>> rows)
    {
        _rows = rows;
        _columns = rows.FirstOrDefault()?.Keys.ToList() ?? new List<string>();
    }

    public override int FieldCount => _columns.Count;
    public override bool HasRows => _rows.Count > 0;
    public override int Depth => 0;
    public override bool IsClosed => false;
    public override int RecordsAffected => 0;

    public override Type GetFieldType(int ordinal)
    {
        if (_rows.Count == 0) return typeof(object);

        var first = _rows[0];
        var name = _columns[ordinal];
        if (first.TryGetValue(name, out var value) && value is not null && value is not DBNull)
        {
            return value.GetType();
        }

        return typeof(object);
    }

    public override bool GetBoolean(int ordinal) => (bool)GetValue(ordinal)!;
    public override byte GetByte(int ordinal) => (byte)GetValue(ordinal)!;
    public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) => throw new NotSupportedException();
    public override char GetChar(int ordinal) => (char)GetValue(ordinal)!;
    public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length) => throw new NotSupportedException();
    public override string GetDataTypeName(int ordinal) => GetFieldType(ordinal).Name;
    public override DateTime GetDateTime(int ordinal) => (DateTime)GetValue(ordinal)!;
    public override decimal GetDecimal(int ordinal) => (decimal)GetValue(ordinal)!;
    public override double GetDouble(int ordinal) => (double)GetValue(ordinal)!;
    public override float GetFloat(int ordinal) => (float)GetValue(ordinal)!;
    public override Guid GetGuid(int ordinal) => (Guid)GetValue(ordinal)!;
    public override short GetInt16(int ordinal) => (short)GetValue(ordinal)!;
    public override int GetInt32(int ordinal) => (int)GetValue(ordinal)!;
    public override long GetInt64(int ordinal) => (long)GetValue(ordinal)!;
    public override string GetName(int ordinal) => _columns[ordinal];
    public override int GetOrdinal(string name) => _columns.IndexOf(name);
    public override string GetString(int ordinal) => (string)GetValue(ordinal)!;
    public override object GetValue(int ordinal) => _rows[_rowIndex][_columns[ordinal]] ?? DBNull.Value;
    public override int GetValues(object[] values)
    {
        for (var i = 0; i < FieldCount; i++)
        {
            values[i] = GetValue(i);
        }

        return FieldCount;
    }

    public override bool IsDBNull(int ordinal) => GetValue(ordinal) is DBNull;
    public override bool Read() => ++_rowIndex < _rows.Count;
    public override Task<bool> ReadAsync(CancellationToken cancellationToken) => Task.FromResult(Read());
    public override bool NextResult() => false;
    public override Task<bool> NextResultAsync(CancellationToken cancellationToken) => Task.FromResult(false);
    public override IEnumerator GetEnumerator() => ((IEnumerable)_rows).GetEnumerator();
    public override object this[int ordinal] => GetValue(ordinal);
    public override object this[string name] => GetValue(GetOrdinal(name));
}

internal sealed class FakeCommandResult
{
    public object? ScalarResult { get; init; }
    public int RowsAffected { get; init; }
    public IReadOnlyList<Dictionary<string, object?>>? Rows { get; init; }

    public static FakeCommandResult Scalar(object? scalar) => new() { ScalarResult = scalar, RowsAffected = 0 };
    public static FakeCommandResult AffectedRows(int rowsAffected) => new() { RowsAffected = rowsAffected, Rows = Array.Empty<Dictionary<string, object?>>() };
    public static FakeCommandResult Reader(IReadOnlyList<Dictionary<string, object?>> rows) => new() { Rows = rows, RowsAffected = 0 };
}

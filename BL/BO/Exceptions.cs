

namespace BO;

[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}


[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}


[Serializable]
public class BlInvalidDataException : Exception
{
    public BlInvalidDataException(string? message) : base(message) { }
}

[Serializable]
public class cannotDeleteException : Exception
{
    public cannotDeleteException(string? message) : base(message) { }
}

public class errorInDateException : Exception
{
    public errorInDateException(string? message) : base(message) { }
}

public class LoopsInDependentTaskEwxeption : Exception
{
    public LoopsInDependentTaskEwxeption(string? message) : base(message) { }
}


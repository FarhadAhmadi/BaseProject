namespace BaseProject.Domain.Entities.Base
{
    /// <summary>
    /// Represents an abstract intermediate entity.
    /// Can be used to share additional logic or properties between specialized entities.
    /// </summary>
    public abstract partial class SubBaseEntity : ParentEntity
    {
        // Additional shared properties or methods for derived entities can be added here.
    }
}

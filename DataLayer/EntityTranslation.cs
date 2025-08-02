using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class EntityTranslation
{
    public int EntityTranslationId { get; set; }

    public string EntityName { get; set; } = null!;

    public int EntityId { get; set; }

    public string Culture { get; set; } = null!;

    public string Property { get; set; } = null!;

    public string Value { get; set; } = null!;
}

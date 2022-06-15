using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping;

namespace Nop.Data.Migrations;

[NopMigration("2022/06/08 22:00:00", "Product export column", MigrationProcessType.Update)]
public class AddExportToProduct : AutoReversingMigration
{
    public override void Up()
    {
        if (!Schema.Table(NameCompatibilityManager.GetTableName(typeof(Product)))
                .Column(nameof(Product.Exported)).Exists())
        {
            Alter.Table(NameCompatibilityManager.GetTableName(typeof(Product)))
                .AddColumn(nameof(Product.Exported))
                .AsBoolean()
                .NotNullable()
                .SetExistingRowsTo(false);
        }
    }
}
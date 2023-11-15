using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniETrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Persistence;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        //TODO-HUS Belki burada DeletedDate'e sahip olan datadan ziyade, Status 0 mı gibi bir kontrol de ekleyebiliriz. Ama tabi silinme tarihi de önemli aslında. Neyse düşünücez.
        builder.HasQueryFilter(b => !b.Deleted.HasValue); //Default olarak bu filtreyi uygulacak. Yani eğer DeletedDate değerine sahipse o datayı getirmeyecek.
        //Burda önemli nokta şu ki, eğer biz bunun iptal olmasını istiyorsak napıcaz? O zaman için de mesela Get Query'lerine bakarsan görüceksin, IgnoreQueryFilters()
        //adında bir method çalıştırıyoruz. Bunu çalıştırdığımız querylerde doğal olarak bize silinmiş datanın da gelmesini sağlıyoruz.

        builder.HasIndex(indexExpression: b => b.Name).IsUnique(); //Name alanına bir indeks vermiş olduk. //Indexin unique olmasının sağladık
    }
}

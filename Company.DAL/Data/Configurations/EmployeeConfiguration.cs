using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.DAL.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Property(E => E.Address).IsRequired();
            builder.Property(E => E.Salary).HasColumnType("decimal(12,2)");
            builder.Property(E => E.Gender).HasConversion(gender => gender.ToString(),genderAsString => (Gender)Enum.Parse(typeof(Gender),genderAsString,true));
            builder.Property(E => E.EmpType).HasConversion(
   (EmpType) => EmpType.ToString(),
   (EmpTypeAsString) => (EmpType)Enum.Parse(typeof(EmpType), EmpTypeAsString, true)
   );
        }
    }
}

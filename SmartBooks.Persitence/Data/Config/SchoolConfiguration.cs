using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartBooks.Domains.SchoolEntities;

namespace SmartBooks.Persitence.Data.Config
{

    public class ClassRoomDetailConfiguration : IEntityTypeConfiguration<ClassRoom>
    {
        public void Configure(EntityTypeBuilder<ClassRoom> builder)
        {
            builder.HasOne(e => e.Block)
                .WithMany(e => e.ClassRooms)
                .HasForeignKey(e => e.BlockId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Level)
                .WithMany(e => e.ClassRooms)
                .HasForeignKey(e => e.LevelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ClassRegisterConfiguration : IEntityTypeConfiguration<ClassRegister>
    {
        public void Configure(EntityTypeBuilder<ClassRegister> builder)
        {
            builder.HasOne(e => e.ClassRoom)
                .WithMany()
                .HasForeignKey(e => e.ClassRoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ClassTeacher)
                .WithMany()
                .HasForeignKey(e => e.ClassTeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.SchoolTerm)
                .WithMany()
                .HasForeignKey(e => e.SchoolTermId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ClassRegisterDetailConfiguration : IEntityTypeConfiguration<ClassRegisterDetail>
    {
        public void Configure(EntityTypeBuilder<ClassRegisterDetail> builder)
        {
            builder.HasOne(e => e.ClassRegister)
                .WithMany(e => e.ClassRegisterDetails)
                .HasForeignKey(e => e.ClassRegisterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class SchoolTermDetailConfiguration : IEntityTypeConfiguration<SchoolTerm>
    {
        public void Configure(EntityTypeBuilder<SchoolTerm> builder)
        {
            builder.HasOne(e => e.SchoolYear)
                .WithMany(e => e.SchoolTerms)
                .HasForeignKey(e => e.SchoolYearId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class StudentDetailConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasOne(e => e.JoinedLevel)
                .WithMany()
                .HasForeignKey(e => e.JoinedLevelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

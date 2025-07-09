using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DealerService;

public partial class DealerDBContext : DbContext
{
    public DealerDBContext()
    {
    }

    public DealerDBContext(DbContextOptions<DealerDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dealer> Dealers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=(local)\\sqlexpress; database=DealerDB; integrated security=sspi; trustservercertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dealer>(entity =>
        {
            entity.HasKey(e => e.DealerId).HasName("PK__Dealers__CA2F8EB225C76F1E");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankIfsccode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("BankIFSCCode");
            entity.Property(e => e.Location)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

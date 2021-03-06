﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace awSetup_Razor.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Codes> Codes { get; set; }
        public virtual DbSet<CustomerContacts> CustomerContacts { get; set; }
        public virtual DbSet<CustomerOptions> CustomerOptions { get; set; }
        public virtual DbSet<CustomerPhones> CustomerPhones { get; set; }
        public virtual DbSet<CustomerRates> CustomerRates { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<MessageTypes> MessageTypes { get; set; }
        public virtual DbSet<ScriptSchedules> ScriptSchedules { get; set; }
        public virtual DbSet<ScriptTags> ScriptTags { get; set; }
        public virtual DbSet<ScriptActions> ScriptActions { get; set; }
        public virtual DbSet<Scripts> Scripts { get; set; }
        public virtual DbSet<DataFiles> DataFiles { get; set; }
        public virtual DbSet<DataRows> DataRows { get; set; }
        public virtual DbSet<DataValues> DataValues { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<MasterRates> MasterRates { get; set; }


        // Unable to generate entity type for table 'dbo.NoCall'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Codes>(entity =>
            {
                entity.HasIndex(e => new { e.Category, e.Code })
                    .HasName("IX_Codes_CategoryCodes");

                entity.HasIndex(e => new { e.ParentCodeId, e.Category });

                entity.Property(e => e.CodeId).ValueGeneratedNever();

                entity.Property(e => e.Category).IsUnicode(false);

                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.Label).IsUnicode(false);

                entity.Property(e => e.MicrosoftCode).IsUnicode(false);

                entity.Property(e => e.ModifiedBy).IsUnicode(false);

                entity.Property(e => e.ToolTipText).IsUnicode(false);

                entity.Property(e => e.TwilioCode).IsUnicode(false);
            });

            modelBuilder.Entity<CustomerContacts>(entity =>
            {
                entity.Property(e => e.Address1).IsUnicode(false);

                entity.Property(e => e.Address2).IsUnicode(false);

                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.ContactName).IsUnicode(false);

                entity.Property(e => e.ContactTypeCode).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerContacts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CustomerContacts_Customers");
            });

            modelBuilder.Entity<CustomerOptions>(entity =>
            {
                entity.Property(e => e.OptionNameCode).IsUnicode(false);

                entity.Property(e => e.OptionTypeCode).IsUnicode(false);

                entity.Property(e => e.OptionValueCode).IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerOptions)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_CustomerOptions_Customers");
            });

            modelBuilder.Entity<CustomerPhones>(entity =>
            {
                entity.Property(e => e.ForwardNumber).IsUnicode(false);

                entity.Property(e => e.TwilioPhoneNumber).IsUnicode(false);

                entity.Property(e => e.UnhandledMessage).IsUnicode(false);
            });

            modelBuilder.Entity<CustomerRates>(entity =>
            {
                entity.HasKey(e => e.CustomerRateId)
                    .HasName("PK_CustomerDeliveryRates");
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.Property(e => e.PrimaryPhone).IsUnicode(false);

                entity.Property(e => e.CustomerCode).IsUnicode(false);

                entity.Property(e => e.CustomerName).IsUnicode(false);

                entity.Property(e => e.EmailAddress).IsUnicode(false);

                entity.Property(e => e.FTPPassword).IsUnicode(false);

                entity.Property(e => e.FTPUserName).IsUnicode(false);

                entity.Property(e => e.SendGridApiKey).IsUnicode(false);

                entity.Property(e => e.TwilioAccountSid).IsUnicode(false);

                entity.Property(e => e.TwilioAuthToken).IsUnicode(false);
            });

            modelBuilder.Entity<MessageTypes>(entity =>
            {
                entity.HasIndex(e => new { e.MessageTypeId, e.MessageLabel, e.FilenameCode, e.Active, e.CustomerId })
                    .HasName("IX_MessageTypes_CustomerID");

                entity.Property(e => e.DeliveryTypeTag).IsUnicode(false);

                entity.Property(e => e.FilenameCode).IsUnicode(false);

                entity.Property(e => e.LanguageTag).IsUnicode(false);

                entity.Property(e => e.MessageLabel).IsUnicode(false);
            });

            modelBuilder.Entity<ScriptSchedules>(entity =>
            {
                entity.HasKey(e => e.ScriptScheduleId)
                    .HasName("PK_DeliverySchedules");

                entity.HasOne(d => d.Script)
                    .WithMany(p => p.ScriptSchedules)
                    .HasForeignKey(d => d.ScriptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScriptSchedules_Scripts");
            });

            modelBuilder.Entity<ScriptTags>(entity =>
            {
                ////entity.HasIndex(e => new { e.FormatCode, e.QueueMapColumn, e.ScriptId, e.TagName })
                //    .HasName("IX_ScriptTags_ScriptID_TagName");

                entity.Property(e => e.DataTypeCode).IsUnicode(false);

                entity.Property(e => e.FormatCode).IsUnicode(false);

                entity.Property(e => e.QueueMapCode).IsUnicode(false);

                entity.Property(e => e.TagName).IsUnicode(false);

                entity.HasOne(d => d.Script)
                    .WithMany(p => p.ScriptTags)
                    .HasForeignKey(d => d.ScriptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScriptTags_Scripts");
            });

            modelBuilder.Entity<ScriptActions>(entity =>
            {
                entity.Property(e => e.Response).IsUnicode(false);
                entity.Property(e => e.ActionCode).IsUnicode(false);
                entity.Property(e => e.Dial).IsUnicode(false);
                entity.Property(e => e.DialTag).IsUnicode(false);
                entity.Property(e => e.ReplyText).IsUnicode(false);
                entity.Property(e => e.StoredProcedure).IsUnicode(false);

                entity.HasOne(d => d.Script)
                    .WithMany(p => p.ScriptActions)
                    .HasForeignKey(d => d.ScriptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScriptActions_Scripts");
            });

            modelBuilder.Entity<Scripts>(entity =>
            {
                entity.Property(e => e.DeliveryTypeCode).IsUnicode(false);

                entity.Property(e => e.LanguageCode).IsUnicode(false);

                entity.Property(e => e.MessagePrefix).IsUnicode(false);

                entity.Property(e => e.MessageScript).IsUnicode(false);

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.Scripts)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Scripts_MessageTypes");
            });
            modelBuilder.Entity<DataFiles>(entity =>
            {
                entity.HasKey(e => e.DataFileId)
                    .HasName("PK_Datafiles");

                entity.HasIndex(e => new { e.CustomerId, e.MessageTypeId });

                entity.Property(e => e.DataFileName).IsUnicode(false);
            });

            modelBuilder.Entity<DataRows>(entity =>
            {
                entity.HasKey(e => e.DataRowId)
                    .HasName("PK_Datarows");

                entity.HasIndex(e => e.DataFileId);

                entity.Property(e => e.DeliveryTypeCode).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.LanguageCode).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);
            });

            modelBuilder.Entity<DataValues>(entity =>
            {
                entity.HasKey(e => e.DataValueId)
                    .HasName("PK_Datavalues");

                entity.HasIndex(e => new { e.DataFileId, e.DataRowId });

                entity.Property(e => e.ColumnName).IsUnicode(false);

                entity.Property(e => e.ColumnValue).IsUnicode(false);
            });

            modelBuilder.Entity<Settings>(entity =>
            {
                entity.Property(e => e.ItemName).IsUnicode(false);

                entity.Property(e => e.ItemValue).IsUnicode(false);
            });

            modelBuilder.Entity<MasterRates>();
        }
    }
}
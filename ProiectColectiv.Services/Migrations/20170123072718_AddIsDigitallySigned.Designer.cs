using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170123072718_AddIsDigitallySigned")]
    partial class AddIsDigitallySigned
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.Document", b =>
                {
                    b.Property<int>("IdDocument")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abstract")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<DateTime>("DateAdded");

                    b.Property<int?>("IdDocumentTask");

                    b.Property<int?>("IdDocumentTemplate");

                    b.Property<DateTime?>("LastModified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("IdDocument");

                    b.HasIndex("IdDocumentTask");

                    b.HasIndex("IdDocumentTemplate");

                    b.HasIndex("UserId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentData", b =>
                {
                    b.Property<int>("IdDocumentData")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.HasKey("IdDocumentData");

                    b.ToTable("DocumentDatas");

                    b.HasDiscriminator<string>("Discriminator").HasValue("DocumentData");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentDataTemplateItem", b =>
                {
                    b.Property<int>("IdDocumentDataTemplateItem")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DocumentDataTemplateIdDocumentData");

                    b.Property<int>("IdDocumentData");

                    b.Property<int>("IdDocumentTemplateItem");

                    b.Property<string>("Value");

                    b.HasKey("IdDocumentDataTemplateItem");

                    b.HasIndex("DocumentDataTemplateIdDocumentData");

                    b.HasIndex("IdDocumentData");

                    b.HasIndex("IdDocumentTemplateItem");

                    b.ToTable("DocumentDataTemplateItems");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentState", b =>
                {
                    b.Property<int>("IdDocumentState")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DocumentStatus");

                    b.Property<int>("IdDocument");

                    b.Property<int>("IdDocumentData");

                    b.Property<bool>("IsDigitallySigned");

                    b.Property<DateTime>("StatusDate");

                    b.Property<double>("Version");

                    b.HasKey("IdDocumentState");

                    b.HasIndex("IdDocument");

                    b.HasIndex("IdDocumentData");

                    b.ToTable("DocumentStates");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTag", b =>
                {
                    b.Property<int>("IdDocumentTag")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("IdDocument");

                    b.Property<int>("IdTag");

                    b.HasKey("IdDocumentTag");

                    b.HasIndex("IdDocument");

                    b.HasIndex("IdTag");

                    b.ToTable("DocumentTags");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTask", b =>
                {
                    b.Property<int>("IdDocumentTask")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<int>("IdDocumentTaskType");

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("IdDocumentTask");

                    b.HasIndex("IdDocumentTaskType");

                    b.HasIndex("UserId");

                    b.ToTable("DocumentTasks");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskState", b =>
                {
                    b.Property<int>("IdDocumentTaskState")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DocumentTaskStatus");

                    b.Property<int>("IdDocumentTask");

                    b.Property<int?>("IdDocumentTaskTypePath");

                    b.Property<DateTime>("StateDate");

                    b.HasKey("IdDocumentTaskState");

                    b.HasIndex("IdDocumentTask");

                    b.HasIndex("IdDocumentTaskTypePath");

                    b.ToTable("DocumentTaskStates");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskTemplate", b =>
                {
                    b.Property<int>("IdDocumentTaskTemplate")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("IdDocumentTemplate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("IdDocumentTaskTemplate");

                    b.HasIndex("IdDocumentTemplate");

                    b.ToTable("DocumentTaskTemplates");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskType", b =>
                {
                    b.Property<int>("IdDocumentTaskType")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DaysLimit");

                    b.Property<int>("IdDocumentTaskTemplate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("IdDocumentTaskType");

                    b.HasIndex("IdDocumentTaskTemplate");

                    b.ToTable("DocumentTaskTypes");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskTypePath", b =>
                {
                    b.Property<int>("IdDocumentTaskTypePath")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("IdDocumentTaskType");

                    b.Property<int?>("IdNextPath");

                    b.Property<int>("IdUserGroup");

                    b.Property<int>("Index");

                    b.HasKey("IdDocumentTaskTypePath");

                    b.HasIndex("IdDocumentTaskType");

                    b.HasIndex("IdNextPath");

                    b.HasIndex("IdUserGroup");

                    b.ToTable("DocumentTaskTypePaths");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTemplate", b =>
                {
                    b.Property<int>("IdDocumentTemplate")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Data");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("IdDocumentTemplate");

                    b.ToTable("DocumentTemplates");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTemplateItem", b =>
                {
                    b.Property<int>("IdDocumentTemplateItem")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("IdDocumentTemplate");

                    b.Property<string>("Label")
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("IdDocumentTemplateItem");

                    b.HasIndex("IdDocumentTemplate");

                    b.ToTable("DocumentTemplateItems");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTemplateItemValue", b =>
                {
                    b.Property<int>("IdDocumentTemplateItemValue")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("IdDocumentTemplateItem");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("IdDocumentTemplateItemValue");

                    b.HasIndex("IdDocumentTemplateItem");

                    b.ToTable("DocumentTemplateItemValues");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.Logs", b =>
                {
                    b.Property<int>("IdLog")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Message")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("UserId");

                    b.HasKey("IdLog");

                    b.HasIndex("UserId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.Tag", b =>
                {
                    b.Property<int>("IdTag")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 25);

                    b.HasKey("IdTag");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.User", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int?>("IdUserGroup");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("IdUserGroup");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.UserGroup", b =>
                {
                    b.Property<int>("IdUserGroup")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("IdUserGroup");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentDataTemplate", b =>
                {
                    b.HasBaseType("ProiectColectiv.Core.DomainModel.Entities.DocumentData");


                    b.ToTable("DocumentDataTemplate");

                    b.HasDiscriminator().HasValue("DocumentDataTemplate");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentDataUpload", b =>
                {
                    b.HasBaseType("ProiectColectiv.Core.DomainModel.Entities.DocumentData");

                    b.Property<byte[]>("Data");

                    b.ToTable("DocumentDataUpload");

                    b.HasDiscriminator().HasValue("DocumentDataUpload");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.Document", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentTask", "DocumentTask")
                        .WithMany("Documents")
                        .HasForeignKey("IdDocumentTask");

                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentTemplate", "DocumentTemplate")
                        .WithMany()
                        .HasForeignKey("IdDocumentTemplate");

                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.User", "User")
                        .WithMany("Documents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentDataTemplateItem", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentDataTemplate")
                        .WithMany("DocumentDataTemplateItems")
                        .HasForeignKey("DocumentDataTemplateIdDocumentData");

                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentData", "DocumentData")
                        .WithMany()
                        .HasForeignKey("IdDocumentData")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentTemplateItem", "DocumentTemplateItem")
                        .WithMany()
                        .HasForeignKey("IdDocumentTemplateItem")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentState", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.Document", "Document")
                        .WithMany("DocumentStates")
                        .HasForeignKey("IdDocument")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentData", "DocumentData")
                        .WithMany()
                        .HasForeignKey("IdDocumentData")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTag", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.Document", "Document")
                        .WithMany("DocumentTags")
                        .HasForeignKey("IdDocument")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.Tag", "Tag")
                        .WithMany("DocumentTags")
                        .HasForeignKey("IdTag")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTask", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskType", "DocumentTaskType")
                        .WithMany()
                        .HasForeignKey("IdDocumentTaskType")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.User", "User")
                        .WithMany("DocumentTasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskState", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentTask", "DocumentTask")
                        .WithMany("DocumentTaskStates")
                        .HasForeignKey("IdDocumentTask")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskTypePath", "DocumentTaskTypePath")
                        .WithMany()
                        .HasForeignKey("IdDocumentTaskTypePath");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskTemplate", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentTemplate", "DocumentTemplate")
                        .WithMany()
                        .HasForeignKey("IdDocumentTemplate")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskType", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskTemplate", "DocumentTaskTemplate")
                        .WithMany("DocumentTaskTypes")
                        .HasForeignKey("IdDocumentTaskTemplate")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskTypePath", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskType", "DocumentTaskType")
                        .WithMany("Paths")
                        .HasForeignKey("IdDocumentTaskType")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentTaskTypePath", "NextPath")
                        .WithMany()
                        .HasForeignKey("IdNextPath");

                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.UserGroup", "UserGroup")
                        .WithMany()
                        .HasForeignKey("IdUserGroup")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTemplateItem", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentTemplate", "DocumentTemplate")
                        .WithMany("DocumentTemplateItems")
                        .HasForeignKey("IdDocumentTemplate")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.DocumentTemplateItemValue", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.DocumentTemplateItem", "DocumentTemplateItem")
                        .WithMany("DocumentTemplateItemValues")
                        .HasForeignKey("IdDocumentTemplateItem")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.Logs", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ProiectColectiv.Core.DomainModel.Entities.User", b =>
                {
                    b.HasOne("ProiectColectiv.Core.DomainModel.Entities.UserGroup", "UserGroup")
                        .WithMany("Users")
                        .HasForeignKey("IdUserGroup");
                });
        }
    }
}

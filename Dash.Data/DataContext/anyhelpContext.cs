

using anyhelp.Data.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace anyhelp.Data.DataContext
{
    public partial class anyhelpContext : DbContext
    {
        public anyhelpContext()
        {
        }

        public anyhelpContext(DbContextOptions<anyhelpContext> options)
            : base(options)
        {
        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
                AppConfiguration appConfig = new AppConfiguration();
                
                optionsBuilder.UseSqlServer(appConfig.SqlConnectonString);
            }
        }

        public virtual DbSet<TblBuyerinquiry> TblBuyerinquiries { get; set; }
        public virtual DbSet<TblBuyernotification> TblBuyernotifications { get; set; }
        public virtual DbSet<TblCategory> TblCategories { get; set; }
        public virtual DbSet<TblPayment> TblPayments { get; set; }
        public virtual DbSet<TblSellercategory> TblSellercategories { get; set; }
        public virtual DbSet<TblSellernotification> TblSellernotifications { get; set; }
        public virtual DbSet<TblSellerregister> TblSellerregisters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("anyhelp")
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblBuyerinquiry>(entity =>
            {
                entity.HasKey(e => e.BuyerinquiryId);

                entity.ToTable("tbl_buyerinquiry", "dbo");

                entity.Property(e => e.BuyerinquiryId).HasColumnName("buyerinquiry_id");

                entity.Property(e => e.BuyerinquiryCategoryid).HasColumnName("buyerinquiry_categoryid");

                entity.Property(e => e.BuyerinquiryFullname)
                    .HasMaxLength(250)
                    .HasColumnName("buyerinquiry_fullname");

                entity.Property(e => e.BuyerinquiryIsdelete).HasColumnName("buyerinquiry_isdelete");

                entity.Property(e => e.BuyerinquiryLatitude)
                    .HasMaxLength(250)
                    .HasColumnName("buyerinquiry_latitude");

                entity.Property(e => e.BuyerinquiryLongitude)
                    .HasMaxLength(250)
                    .HasColumnName("buyerinquiry_longitude");

                entity.Property(e => e.BuyerinquiryPhoneno)
                    .HasMaxLength(400)
                    .HasColumnName("buyerinquiry_phoneno");
            });

            modelBuilder.Entity<TblBuyernotification>(entity =>
            {
                entity.HasKey(e => e.BuyernotificationId);

                entity.ToTable("tbl_buyernotification", "dbo");

                entity.Property(e => e.BuyernotificationId)
                    .ValueGeneratedNever()
                    .HasColumnName("buyernotification_id");

                entity.Property(e => e.BuyernotificationBuyersellerid).HasColumnName("buyernotification_buyersellerid");

                entity.Property(e => e.BuyernotificationCategoryid).HasColumnName("buyernotification_categoryid");

                entity.Property(e => e.BuyernotificationDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("buyernotification_datetime");

                entity.Property(e => e.BuyernotificationIsdelete).HasColumnName("buyernotification_isdelete");

                entity.Property(e => e.BuyernotificationIsread).HasColumnName("buyernotification_isread");

                entity.Property(e => e.BuyernotificationIsselleraccepted).HasColumnName("buyernotification_isselleraccepted");

                entity.Property(e => e.BuyernotificationMessage)
                    .HasMaxLength(2000)
                    .HasColumnName("buyernotification_message");

                entity.Property(e => e.BuyernotificationParentid).HasColumnName("buyernotification_parentid");
            });

            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.ToTable("tbl_category", "dbo");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CategoryIsdelete).HasColumnName("category_isdelete");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(400)
                    .HasColumnName("category_name");
            });

            modelBuilder.Entity<TblPayment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);

                entity.ToTable("tbl_payment", "dbo");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");

                entity.Property(e => e.PaymentDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("payment_datetime");

                entity.Property(e => e.PaymentIsdelete).HasColumnName("payment_isdelete");

                entity.Property(e => e.PaymentIspaid).HasColumnName("payment_ispaid");

                entity.Property(e => e.PaymentSellerid).HasColumnName("payment_sellerid");

                entity.Property(e => e.PaymentTransactiondescription)
                    .HasMaxLength(2000)
                    .HasColumnName("payment_transactiondescription");

                entity.Property(e => e.PaymentTransactionid)
                    .HasMaxLength(250)
                    .HasColumnName("payment_transactionid");
            });

            modelBuilder.Entity<TblSellercategory>(entity =>
            {
                entity.HasKey(e => e.SellercategoryId);

                entity.ToTable("tbl_sellercategory", "dbo");

                entity.Property(e => e.SellercategoryId).HasColumnName("sellercategory_id");

                entity.Property(e => e.SellercategoryCategoryid).HasColumnName("sellercategory_categoryid");

                entity.Property(e => e.SellercategoryIsactive).HasColumnName("sellercategory_isactive");

                entity.Property(e => e.SellercategorySellerid).HasColumnName("sellercategory_sellerid");
            });

            modelBuilder.Entity<TblSellernotification>(entity =>
            {
                entity.HasKey(e => e.SellernotificationId);

                entity.ToTable("tbl_sellernotification", "dbo");

                entity.Property(e => e.SellernotificationId).HasColumnName("sellernotification_id");

                entity.Property(e => e.SellernotificationBuyernotificationid).HasColumnName("sellernotification_buyernotificationid");

                entity.Property(e => e.SellernotificationCategoryid).HasColumnName("sellernotification_categoryid");

                entity.Property(e => e.SellernotificationInqueryid).HasColumnName("sellernotification_inqueryid");

                entity.Property(e => e.SellernotificationIsdelete).HasColumnName("sellernotification_isdelete");

                entity.Property(e => e.SellernotificationIsread).HasColumnName("sellernotification_isread");

                entity.Property(e => e.SellernotificationMessage)
                    .HasMaxLength(2000)
                    .HasColumnName("sellernotification_message");

                entity.Property(e => e.SellernotificationSellerid).HasColumnName("sellernotification_sellerid");
            });

            modelBuilder.Entity<TblSellerregister>(entity =>
            {
                entity.HasKey(e => e.SellerregisterId);

                entity.ToTable("tbl_sellerregister", "dbo");

                entity.Property(e => e.SellerregisterId).HasColumnName("sellerregister_id");

                entity.Property(e => e.SellerregisterCreditamount).HasColumnName("sellerregister_creditamount");

                entity.Property(e => e.SellerregisterFullname)
                    .HasMaxLength(250)
                    .HasColumnName("sellerregister_fullname");

                entity.Property(e => e.SellerregisterIsdelete).HasColumnName("sellerregister_isdelete");

                entity.Property(e => e.SellerregisterLatitude)
                    .HasMaxLength(250)
                    .HasColumnName("sellerregister_latitude");

                entity.Property(e => e.SellerregisterLongitude)
                    .HasMaxLength(250)
                    .HasColumnName("sellerregister_longitude");

                entity.Property(e => e.SellerregisterPassword)
                    .HasMaxLength(400)
                    .HasColumnName("sellerregister_password");

                entity.Property(e => e.SellerregisterPhoneno)
                    .HasMaxLength(400)
                    .HasColumnName("sellerregister_phoneno");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

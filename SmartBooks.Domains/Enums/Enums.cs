namespace SmartBooks.Domains.Enums
{
    public enum CompanyType
    {
        General,
        Distribution,
        Sacco,
        School
    }

    public enum UserType
    {
        General,
        Admin
    }

    public enum MainAccountType
    {
        Asset,
        Liability,
        Equity,
        Income,
        Expense
    }
    public enum AccountType
    {
        ALL = 0,
        /// <summary>
        /// Balance sheet current asset sub category in the asset category
        /// Under cash and cash equivalents
        /// </summary>
        Cash,
        /// <summary>
        /// Balance sheet current asset sub category in the asset category
        /// </summary>
        CurrentAsset,
        /// <summary>
        /// Balance sheet current asset sub category in the asset category
        /// </summary>
        OtherAsset,
        /// <summary>
        /// Balance sheet current asset sub category in the asset category
        /// </summary>
        AccountsReceivable,
        /// <summary>
        /// Balance sheet current asset sub category in the asset category
        /// </summary>
        Inventory,
        /// <summary>
        /// Balance sheet long term asset sub category in the asset category
        /// </summary>
        FixedAsset,
        /// <summary>
        /// Balance sheet long term asset sub category in the asset category
        /// It's shown as a negative value to reduce the value of depreciated asset,
        /// so as to reflect the current value of the long term assets.
        /// </summary>
        AccumulatedDepreciation,
        /// <summary>
        /// Balance sheet sub category of current liabilities in the liabilities sections
        /// These are liabilities that are to be paid in the next 12 months.
        /// </summary>
        CurrentLiability,
        /// <summary>
        /// Balance sheet sub category of accounts payable in the liabilities sections
        /// These are liabilities that are to be paid in the next 12 months.
        /// </summary>
        AccountsPayable,
        /// <summary>
        /// Balance sheet sub category of long term liabilities in the liabilities sections
        /// </summary>
        LongTermLiability,
        /// <summary>
        /// Balance sheet's equity category.
        /// This is the networth of the company
        /// </summary>
        Equity,
        /// <summary>
        /// Used for accounts that are used to record companies primary income source
        /// </summary>
        Income,
        /// <summary>
        /// Used for secondary income
        /// it is added to operating profit to get pretax income on the income statement
        /// </summary>
        OtherIncome,
        /// <summary>
        /// Accounts used to record cost of goods
        /// </summary>
        CostofGoods,
        /// Used for all expenses that are subtracted to gross profit to get
        /// opearating profit on income statement
        /// </summary>
        Expense,
        /// <summary>
        /// Use for accounts used only for non operating expenses
        /// These are subtracted from operating profit to get pretax income
        /// </summary>
        OtherExpense
    }

    public enum DetailAccountType
    {
        Bank,
        PettyCash,
        MobileMoney
    }

    public enum SubLedgerType
    {
        Customer,
        Supplier,
        Member,
        Student
    }

    public enum TransactionType
    {
        GeneralJournal,
        Quotation,
        Invoice,
        CashSale,
        CreditNote,
        ReceivePayment,
        PurchaseOrder,
        ReceiveGoods,
        Bill,
        DebitNote,
        MakePayment,
    }

    public enum InventoryItemType
    {
        Inventory,
        Service,
        NonInventory,
        Product,
        SystemItem
    }

    public enum UomType
    {
        Count,
        Weight,
        Length,
        Area,
        Volume,
        Time
    }

    public enum InventoryTransactionType
    {
        Receive,
        Issue,
        Move,
        StockTake
    }

    public enum ReportingMethod
    {
        Accrual,
        Cash
    }

    public enum OrganisationType
    {
        Customer,
        Supplier,
        TaxAgency
    }
    
    public enum RecordType
    {
        User,
        Bin,
        CostCenter,
        CurrencyConversion,
        Customer,
        CustomerProject,
        CustomerType,
        Employee,
        InventoryCategory,
        InventoryItem,
        LedgerAccount,
        OrganisationAddress,
        PaymentTerm,
        ReorderLevel,
        Supplier,
        SupplierType,
        Tax,
        TaxRate,
        UnitofMeasure,
        UomConversion,
        Warehouse,
        SchoolBlock,
        ClassRegister,
        ClassRegisterDetail,
        ClassRoom,
        Dormitory,
        SchoolLevel,
        SchoolTerm,
        SchoolYear,
        Student,
        Subject,
        Teacher,
        TeachingDepartment,
        UtilityRoom,
        Member,
        SaccoFee,
        MemberAccount
    }

    public enum ActionType
    {
        Create,
        Update,
        Delete,
        Login
    }

    public enum LogType
    {
        Info,
        Warn,
        Debug,
        Error
    }

    public enum TransactionStatus
    {
        Draft,
        Checked,
        Authorised,
        Posted,
        Rejected
    }

    public enum SchoolLevel
    {
        PP1,
        PP2,
        Grade1,
        Grade2,
        Grade3,
        Grade4,
        Grade5,
        Class6,
        Class7,
        Class8,
        Form1,
        Form2,
        Form3,
        Form4
    }

    public enum Gender
    {
        Female,
        Male,
        UnSpecified
    }


    public enum MaritalStatus
    {
        Single,
        Married,
        Divorced,
        widowed
    }

    public enum MemberStatus
    {
        /// <summary>
        /// Member as registered
        /// At this point member can edit and upload attachment on their profile
        /// </summary>
        Entered,
        /// <summary>
        /// Member registration has been approved
        /// Any changes to member details are by member are subject to approval
        /// </summary>
        Approved,
        /// <summary>
        /// Member has paid registration fees if any
        /// </summary>
        Active,
        OnHold,
        Rejected,
        Left
    }

    public enum OccupationType
    {
        Employed,
        SelfEmployed,
        EmployedInBussiness
    }

    public enum LearntAboutUs
    {
        Member,
        Friend,
        Website,
        Advertisement
    }

    public enum ApprovalAction
    {
        Approved,
        OnHold,
        Rejected
    }
    public enum PaymentMethodType
    {
        Cash,
        Cheque,
        Pd_Cheque
    }

    public enum AttachmentType
    {
        IdFront,
        IdBack,
        Avator,
        Signature,
        Other
    }

    public enum PaymentFrequency
    {
        Once,
        Daily,
        Weekly,
        Fortnightly,
        Monthly,
        Quarterly,
        HalfYear,
        Yearly
    }

    public enum PaymentItemType
    {
        Fees,
        Shares,
        Savings,
        Loan
    }

    public enum SaccoFeesType
    {
        Registration,
        UserDefined,
        Clearance
    }

    public enum InterestType
    {
        Flat,
        FixedPrincipal,
        Declining
    }

    public enum MemberAccountStatus
    {
        Active,
        Suspended
    }

}
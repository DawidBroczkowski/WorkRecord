using System.Runtime.Serialization;

namespace WorkRecordGui.Models
{
    public enum Position
    {
        [EnumMember(Value = "Dentist")]
        dentist,
        [EnumMember(Value = "Nurse")]
        nurse,
        [EnumMember(Value = "Receptionist")]
        receptionist,
        [EnumMember(Value = "Manager")]
        manager
    }

    public enum LeaveType
    {
        [EnumMember(Value = "Wypoczynkowy")]
        paid,
        [EnumMember(Value = "Bezpłatny")]
        unpaid,
        [EnumMember(Value = "Szkoleniowy")]
        training,
        [EnumMember(Value = "Na żądanie")]
        onDemand,
        [EnumMember(Value = "Okolicznościowy")]
        special,
        [EnumMember(Value = "Opieka nad dzieckiem")]
        childcare,
        [EnumMember(Value = "Siła wyższa")]
        higherPower,
        [EnumMember(Value = "Macierzyński")]
        maternity,
        [EnumMember(Value = "Wychowawczy")]
        parental,
        [EnumMember(Value = "Chorobowy")]
        sick
    }

    public enum Role
    {
        [EnumMember(Value = "User")]
        user,
        [EnumMember(Value = "Coordinator")]
        coordinator,
        [EnumMember(Value = "Manager")]
        manager,
        [EnumMember(Value = "Admin")]
        admin
    }
}

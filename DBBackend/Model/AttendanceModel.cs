namespace DBBackend.Model
{
    public class AttendanceModel
    {
        public int ID { get; set; }
        public int Reg_Ref_ID { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public double THour { get; set; }
    }
}

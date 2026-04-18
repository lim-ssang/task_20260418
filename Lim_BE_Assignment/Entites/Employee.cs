namespace Lim_BE_Assignment.Entites
{
    /// <summary>
    /// 직원
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// 직원 성명
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 이메일
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 연락처
        /// </summary>
        public string TelNo { get; set; }

        /// <summary>
        /// 등록일
        /// </summary>
        public string JoinedDate { get; set; }
    }
}

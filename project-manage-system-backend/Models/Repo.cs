namespace project_manage_system_backend.Models
{
    public class Repo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        /// 實際 repository (第三方) 的擁有者名稱
        public string Owner { get; set; }
        public Project Project { get; set; }
        /// 有無sonarqube
        public bool IsSonarqube { get; set; }
        /// foramt: "帳號:密碼"
        /// encryption: base64
        public string AccountColonPw { get; set; }
        /// Sonarqube url
        public string SonarqubeUrl { get; set; }
        /// sonarqube projectkey
        public string ProjectKey { get; set; }
        /// gitlab repository id
        public string RepoId { get; set; }
        public string Type { get; set; }
    }
}

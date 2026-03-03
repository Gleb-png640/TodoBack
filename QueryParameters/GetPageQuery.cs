namespace TodoBack.QueryParameters {
    public class GetPageQuery {

        public bool? isDone { get; set; } 

        public int page { get; set; }
        public int pageSize { get; set; }

    }
}

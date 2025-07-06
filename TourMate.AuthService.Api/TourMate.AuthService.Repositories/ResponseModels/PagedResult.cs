namespace TourMate.AuthService.Repositories.ResponseModels;

public class PagedResult<T>
{
    public IList<T> Data { get; set; }              // Mảng dữ liệu của trang hiện tại
    public int TotalCount { get; set; }             // Tổng số bản ghi
    public int Page { get; set; }                   // Trang hiện tại
    public int PerPage { get; set; }                // Số bản ghi/trang
    public int TotalPages { get; set; }             // Tổng số trang
    public bool HasNext { get; set; }               // Còn trang sau không
    public bool HasPrevious { get; set; }           // Còn trang trước không
}

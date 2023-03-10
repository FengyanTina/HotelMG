using Dapper;
using MySqlConnector;
using System.Data;
public class ReviewData
{

    MySqlConnection connection;

    public ReviewData()
    {
        connection = new MySqlConnection(("Server=13.51.47.91;Database=hotelmg;Uid=root;Pwd=i-077e801baa9e32977;"));
    }

    public void Open()
    {
        try
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }
        catch (Exception)
        {
            throw new FieldAccessException();
        }
    }

    public List<Review> GetReviewList()
    {
        Open();
        var reviews = connection.Query<Review>("SELECT review_id, reservation_id, customer_fname, review_content FROM reviews INNER JOIN customers ON customers.customer_id=reviews.customer_id;").ToList();
        return reviews;
    }

    public int InsertReview(int customerID, int reservationID, string content)
    {
        Open(); 
        var r = new DynamicParameters();
        r.Add("@customer_id", customerID);
        r.Add("@reservation_id", reservationID);
        r.Add("@review_content", content);
        string sql = $@"INSERT INTO reviews (customer_id, reservation_id, review_content) VALUES (@customer_id,@reservation_id,@review_content); SELECT LAST_INSERT_ID() ";
        int Id = connection.Query<int>(sql, r).First();
        return Id;
    }

    public void DeleteReviewById(int number)
    {
        Open();
        var deleteReview = connection.Query<Review>($"DELETE FROM reviews WHERE review_id = {number};");
    }
}
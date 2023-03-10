using Dapper;
using MySqlConnector;
using System.Data;

public class ReservationData
{

    MySqlConnection connection;

    public ReservationData()
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
        catch (Exception )
        {

            throw new FieldAccessException();
        }
    }

    public List<Reservation> GetReservationDetailList()
    {
        var getResData = connection.Query<Reservation>(@"SELECT reservation_id, reservations.room_id, roomtype.roomType_name, roomstatus.roomStatus_name, room_price, reservations.date_in, reservations.date_out, date_range, reservation_totalpay, customers.customer_fname, customers.customer_fname,reservations.employee_id, reservations.reservation_date
        FROM ((((reservations
        INNER JOIN rooms ON rooms.room_id = reservations.room_id)
        INNER JOIN roomtype ON roomtype.roomType_id = rooms.roomType_id)
        INNER JOIN customers ON customers.customer_id = reservations.customer_id)
        INNER JOIN roomstatus ON rooms.roomStatus_id = rooms.roomStatus_id);").ToList();
        return getResData;
    }

    public List<Reservation> GetReservationList()
    {
        var getRes = connection.Query<Reservation>("SELECT * FROM `reservations`;").ToList();
        return getRes;
    }

    public int MakeReservation(int customer_id, int employeeIdBooking, int room_id, DateTime reservation_date, DateTime date_in, DateTime date_out, double date_range, double totalPay)
    {
        string sql = $"INSERT INTO `reservations`(`customer_id`, `employee_id`, `room_id`, `reservation_date`, `date_in`, `date_out`, date_range, reservation_totalpay) VALUES ('{customer_id}',  '{employeeIdBooking}', '{room_id}', '{reservation_date}','{date_in}','{date_out}', {date_range}, {totalPay}); SELECT LAST_INSERT_ID() ";
        int id = connection.QuerySingle<int>(sql);
        return id;
    }

    public void UpdateReservationDate(int reservation_id, DateTime date_in, DateTime date_out,double date_range, double newTotalPay)
    {
        var updateReservation = connection.Query<Reservation>($"UPDATE `reservations` SET `date_in`='{date_in}',`date_out`='{date_out}', `date_range`={date_range}, `reservation_totalpay`={newTotalPay} WHERE reservations.reservation_id = {reservation_id};");
    }

    public void UpdateReservationRoom(int reservation_id, int room_id)
    {
        var updateReservation = connection.Query<Reservation>($"UPDATE `reservations` SET room_id= {room_id} WHERE reservations.reservation_id ={reservation_id};");
    }

    public Reservation GetSingleReservationById(int reservation_id)//same quary as ListDetail
    {
        var getSingleResrvation = connection.QuerySingle<Reservation>($@"SELECT reservation_id, customer_fname,customer_lname,employee_id,rooms.room_id,roomType_name,room_price, reservation_date, date_in, date_out,date_range,reservation_totalpay FROM(((reservations INNER JOIN customers ON customers.customer_id = reservations.customer_id)INNER JOIN rooms ON rooms.room_id = reservations.room_id)INNER JOIN roomtype ON roomtype.roomType_id=rooms.roomType_id)WHERE reservations.reservation_id = {reservation_id};");
        return getSingleResrvation;
    }

    public List<Reservation> GetTimeSpanData(int reservation_id)
    {
        var getTimeSpan = connection.Query<Reservation>($"SELECT * FROM `reservations`WHERE reservations.reservation_id = {reservation_id};").ToList();
        return getTimeSpan;
    }

    public double GetRoomPrice(int reservation_id) 
    {
        var result = connection.QuerySingle<Reservation> ($"SELECT room_price FROM  reservations INNER JOIN rooms ON reservations.room_id = rooms.room_id WHERE reservations.reservation_id = {reservation_id};");
        return result.room_price;
    }

    public int GetRoomByReservId(int reservation_id) 
    {
        var result = connection.QuerySingle<Reservation> ($"SELECT reservations.room_id FROM  reservations INNER JOIN rooms ON reservations.room_id = rooms.room_id WHERE reservations.reservation_id = {reservation_id};");
        return result.room_id;
    }

     public int GetReservIdByRoomId(int room_id) 
    {
        var result = connection.QuerySingle<Reservation> ($"SELECT reservation_id FROM  reservations INNER JOIN rooms ON reservations.room_id = rooms.room_id WHERE reservations.room_id = {room_id};");
        return result.reservation_id;
    }
}
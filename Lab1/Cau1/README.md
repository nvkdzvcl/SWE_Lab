# Cau 1 - Hoan tat don hang theo SOLID

Chuong trinh console C#, .NET 8, khong can thu vien ngoai hay MySQL.

## Chay tu thu muc Lab1

```powershell
dotnet run --project .\Cau1
dotnet run --project .\Cau1 -- --test
```

Demo in trang thai `COMPLETED`, so du thuong `30.000 VND` cua nguoi gioi
thieu, tu choi don chua thanh toan, hoan tat don khong co nguoi gioi thieu,
va doi sang chinh sach VIP nhan 150 diem.

## Doc code theo thu tu

1. `Domain.cs`: `Order` bao ve chuyen trang thai PENDING -> PAID -> COMPLETED.
   `Customer` giu so du thuong va diem; du lieu dau vao duoc kiem tra.
2. `Services.cs`: ba interface tach luu don, thuong, thong bao.
   `OrderService` nhan chung qua constructor va chi dieu phoi:
   Complete, Save, Apply, NotifyCompleted.
3. `Program.cs`: tao cac lop cu the, truyen vao dich vu va chay minh hoa.
4. `SelfTests.cs`: kiem tra ket qua, tu choi trang thai sai va du lieu sai.

## Doi chieu yeu cau va SOLID

| Nguyen ly | Cach ap dung |
| --- | --- |
| SRP | Order quan ly trang thai; repository luu don; policy thuong; notification thong bao. |
| OCP | Them VipPointsRewardPolicy va doi doi tuong truyen vao, khong sua OrderService. |
| LSP | Cac policy cung thuc hien hop dong Apply: ap dung thuong neu du dieu kien; neu khong thi khong thuong. |
| ISP | Moi interface chi chua mot thao tac can thiet. |
| DIP | OrderService chi phu thuoc interface, khong phu thuoc console, MySQL hay chinh sach 2%. |

`Order.Complete()` tu choi moi trang thai khac PAID, ke ca COMPLETED.
Vi vay goi hoan tat lai cung doi tuong khong the cong thuong lan hai.
`ReferralRewardPolicy` cong 2% vao so du nguoi gioi thieu neu co:
1.500.000 x 0,02 = 30.000 VND. Tien dung `decimal`.

De doi co so du lieu, them lop trien khai OrderRepository. De doi kenh thong
bao, them lop trien khai NotificationService. Chon lop moi tai Program.cs.

## Pham vi minh hoa

- VIP: gia dinh 1 diem cho moi 10.000 VND tron; de khong quy dinh ty le.
  Demo thay policy, khong ap dung dong thoi hai policy.
- Don va so du chi nam trong bo nho, mat khi thoat chuong trinh.
- Chua co transaction, retry hay xu ly dong thoi. Neu dung dich vu luu tru
  hoac gui thong bao that, can bo sung transaction va co che chong thuong
  trung ben vung; demo hien tai khong dam bao rollback khi mot buoc bi loi.

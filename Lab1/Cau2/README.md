# Cau 2 - ProductSearchCriteria voi Builder Pattern

De cau 2 ghi Java **va** C#, nen co hai ban tuong duong.
Can .NET SDK 8 va JDK 11 tro len; khong can thu vien ngoai.

## Chay tu thu muc Lab1

C#:

```powershell
dotnet run --project .\Cau2\CSharp
dotnet run --project .\Cau2\CSharp -- --test
```

Java:

```powershell
javac -d .\Cau2\Java\out .\Cau2\Java\ProductSearchCriteria.java .\Cau2\Java\Main.java .\Cau2\Java\SelfTests.java
java -cp .\Cau2\Java\out Main
java -cp .\Cau2\Java\out SelfTests
```

Ket qua demo cua ca hai ban:

```text
=== Tieu chi tim kiem san pham ===
Keyword: chair
Category: furniture
MinPrice: 500.000 VND
MaxPrice: 5.000.000 VND
Page: 2
PageSize: 5
```

## Doi chieu yeu cau

1. Constructor cua ProductSearchCriteria la private, chi nhan mot Builder.
2. Builder nam trong ProductSearchCriteria; moi ham cau hinh tra ve chinh
   Builder de goi fluent. Mac dinh Page = 1, PageSize = 5.
3. Build()/build() tu choi Page < 1, PageSize ngoai 1..100, gia am,
   va MinPrice > MaxPrice khi ca hai can duoc cung cap.
4. Keyword va Category duoc xoa khoang trang hai dau, gom chuoi khoang
   trang thanh mot dau cach va chuyen chu thuong khong phu thuoc locale.
   Gia tri null hoac chuoi rong sau chuan hoa nghia la khong loc truong do.
5. Ket qua bat bien: C# chi co getter; Java dung private final va getter.
   Sua Builder sau Build khong thay doi doi tuong da tao.

Gia dung decimal trong C#, BigDecimal trong Java. MinPrice va MaxPrice
co the bo trong doc lap; ca hai deu bi tu choi neu am.

## Doc code

- CSharp/Program.cs va Java/Main.java: client tao tieu chi dung nhu de.
- ProductSearchCriteria: du lieu bat bien va nested Builder.
- SelfTests: kiem tra mau de, mac dinh, chuan hoa, bien hop le/khong hop le,
  va tinh bat bien khi tai su dung Builder.

Pham vi bai la xay dung tieu chi, chua truy van danh sach san pham.
ProductRepository trong so do la noi nhan tieu chi khi tich hop tim kiem;
chi can them repository khi can chay loc va phan trang du lieu that.

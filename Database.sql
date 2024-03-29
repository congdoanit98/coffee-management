create database coffeemanament;

use coffeemanament;

-- food
-- table
-- foodCategory
-- account
-- bill
-- billInfo

create table tablefood (
	id int identity primary key,
	name nvarchar(100) not null default N'Chưa có tên',
	statuss nvarchar(100) not null default N'Trống',
)

create table account(
	username nvarchar(100) primary key,
	displayname nvarchar(100)not null,
	pass nvarchar(100) not null default 0,
	typeaccount int not null default 0 --1: admin, 0:staff
)

create table foodcategory(
	id int identity primary key,
	name nvarchar(100) not null default N'Chưa đặt tên',
)

create table food(
	id int identity primary key,
	name nvarchar(100) not null default N'Chưa đặt tên' ,
	idcategory int not null,
	price float not null,
	
	foreign key (idcategory) references foodcategory(id)
)

create table bill (
	id int identity primary key,
	datecheckin date not null default getdate(),
	datecheckout date,
	idtable int not null,
	statuss int not null default 0, --1: thanh toan, 0: chua thanh toan

	foreign key (idtable) references tablefood(id)
)

create table billinfo (
	id int identity primary key,
	idbill int not null,
	idfood int not null,
	countt int not null default 0 ,
	
	foreign key (idbill) references bill(id),
	foreign key (idfood) references food(id)
)

insert into account (username, displayname, pass,typeaccount) values('doan','congdoan','123456',1)
insert into account (username, displayname, pass,typeaccount) values('staff','staff','1',0)
insert into account (username, displayname, pass,typeaccount) values('nhanvien','Tony','1',2)

select * from account

create PROC USP_GetAccountByUserName
@username nvarchar(100)
as 
begin
	select * from Account where username = @username
end

exec USP_GetAccountByUserName @username = N'doan'

create proc USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
as
begin
	select * from account where username = @userName and pass = @passWord
end

-- thêm bàn
declare @i int = 1
while @i < =10
begin
	insert into tablefood (name) values (N'Bàn ' + CAST(@i as nvarchar(100)))
	set @i = @i +1
end

select * from tablefood

-- thêm category
insert into foodcategory(name) values (N'Hải sản')
insert into foodcategory(name) values (N'Nông sản')
insert into foodcategory(name) values (N'Lâm sản')
insert into foodcategory(name) values (N'Sản sản')
insert into foodcategory(name) values (N'Nước')

-- thêm món ăn
insert into food(name,idcategory,price) values (N'Mực nướng sa tê',1,12000)
insert into food(name,idcategory,price) values (N'Nghêu hấp xả',1,50000)
insert into food(name,idcategory,price) values (N'Vú heo nướng',2,60000)
insert into food(name,idcategory,price) values (N'Gà chiên nước mắm',2,45000)
insert into food(name,idcategory,price) values (N'Vịt nướng chao',2,56000)
insert into food(name,idcategory,price) values (N'Heo rừng nướng',3,80000)
insert into food(name,idcategory,price) values (N'Cơm chiên haha',4,50000)
insert into food(name,idcategory,price) values (N'7 Up',5,10000)
insert into food(name,idcategory,price) values (N'Cafe',5,12000)

--thêm bill
insert into bill(datecheckin,datecheckout,idtable,statuss) values (GETDATE(), null, 1, 0)
insert into bill(datecheckin,datecheckout,idtable,statuss) values (GETDATE(), null, 2, 0)
insert into bill(datecheckin,datecheckout,idtable,statuss) values (GETDATE(), null, 2, 1)

-- thêm info
insert into billinfo(idbill,idfood,countt) values (1,1,2)
insert into billinfo(idbill,idfood,countt) values (1,3,4)
insert into billinfo(idbill,idfood,countt) values (1,5,1)
insert into billinfo(idbill,idfood,countt) values (2,2,2)
insert into billinfo(idbill,idfood,countt) values (3,6,1)

update tablefood set statuss = N'Có người' where id = 5

create proc USP_InsertBill
@idTable int
as
begin
	insert into bill(datecheckin, datecheckout, idtable, statuss, discount) values (GETDATE(), null, @idTable, 0,0) 
end 

create proc USP_GetTableList
as select * from tablefood

exec USP_GetTableList

---------------------------
create PROC USP_InsertBillInfo
@idBill int, @idFood int, @count int
as
begin
	declare @isExitsBillInfo int;
	declare @foodCount int = 1;
	
	select @isExitsBillInfo = id, @foodCount = b.countt from billinfo as b where idbill = @idBill and idfood = @idFood
	
	if(@isExitsBillInfo > 0)
	begin
		declare @newCount int = @foodCount + @count
		if(@newCount > 0)
			update billinfo set countt = @foodCount + @count where idfood = @idFood
		else
			delete billinfo where idbill = @idBill and idfood = @idFood
	end
	else
	begin
		insert into billinfo(idbill, idfood, countt) values (@idBill, @idFood,@count)
	end
end
delete billinfo
delete bill
---------------------------
alter trigger UTG_UpdateBillInfo
on billinfo for insert, update
as
begin
	declare @idBill int
	
	select @idBill = idbill from inserted
	 
	declare @idTable int
	
	select @idTable = idtable from bill where id = @idBill and statuss = 0
	
	declare @count int
	select @count = COUNT(*) from billinfo where idbill = @idBill
	
	if(@count > 0)
		update tablefood set statuss = N'Có người' where id = @idTable 
	else
		update tablefood set statuss = N'Trống' where id = @idTable
end 
----------------------------------

----------------------------------
create trigger UTG_UpdateBill
on bill for update
as
begin
	declare @idBill int
	
	select @idBill = id from inserted
	
	declare @idTable int
	
	select @idTable = idtable from bill where id = @idBill
	
	declare @count int = 0
	
	select @count = COUNT(*) from bill where idtable = @idTable and statuss = 0
	
	if(@count = 0)
		update tablefood set statuss = N'Trống' where id = @idTable
end
----------------------------

alter table bill add discount int

alter PROC USP_SwitchTable
@idTable1 int, @idTable2 int
as
begin
	declare @idFirstBill int
	declare @idSecondBill int
	
	declare @isFirstTableEmty int = 1
	declare @isSecondTableEmty int = 1
	
	select @idFirstBill = id from bill where idtable= @idTable1 and statuss = 0
	select @idSecondBill = id from bill where idtable= @idTable2 and statuss = 0
	
	if(@idFirstBill is null)
	begin
		insert into bill (datecheckin,datecheckout,idtable,statuss)
			values(GETDATE(), null, @idTable1,0)
		select @idFirstBill = max(id) from bill where idtable = @idTable1 and statuss = 0
	end
	
	select @isFirstTableEmty = COUNT(*) from billinfo where idbill = @idFirstBill
	
	if(@idSecondBill is null)
	begin
		insert into bill (datecheckin,datecheckout,idtable,statuss)
			values(GETDATE(), null, @idTable2,0)
		select @idSecondBill = max(id) from bill where idtable = @idTable2 and statuss = 0	
	end
	
	select @isSecondTableEmty = COUNT(*) from billinfo where idbill = @idSecondBill
	
	select id into IDBillInfoTable from billinfo where idbill = @idSecondBill
	
	update billinfo set idbill = @idSecondBill where idbill = @idFirstBill
	
	update billinfo set idbill = @idFirstBill where id in (select * from IDBillInfoTable)
	
	
	
	if(@isFirstTableEmty = 0)
		update tablefood set statuss = N'Trống' where id = @idTable2
		
	if(@isSecondTableEmty = 0)
		update tablefood set statuss = N'Trống' where id = @idTable1
		
	drop table IDBillInfoTable 
end

update tablefood set statuss = N'Trống'

alter table bill add totalPrice float

select * from bill
select * from billinfo
select * from tablefood



create PROC USP_GetListBillByDate
@checkin date, @checkout date
as
begin
	select t.name as [Tên bàn], b.totalPrice as [Tổng tiền], b.datecheckin as[Ngày vào], b.datecheckout as [Ngày ra], b.discount as [Giảm giá]
	from bill as b,tablefood as t 
	where datecheckin >= @checkin and datecheckout <= @checkout and b.statuss = 1 and b.idtable = t.id
end

create PROC USP_UpdateAccount
@userName nvarchar(100), @displayName nvarchar(100), @password nvarchar(100), @newPassword nvarchar(100)
as
begin
	declare @isRightPass int = 0
	
	select @isRightPass = COUNT(*) from account where username = @userName and pass = @password
	
	if(@isRightPass = 1)
	begin
		if(@newPassword = null or @newPassword = '')
		begin
			update account set displayname = @displayName where username = @userName
		end
		else
			update account set displayname = @displayName, pass = @newPassword where username = @userName
		
	end
end

create trigger UTG_DeleteBillInfo
on billinfo for delete
as
begin
	declare @idBillInfo int
	declare @idBill int
	select @idBillInfo = id, @idBill = deleted.idbill from deleted
	
	declare @idTable int
	select @idTable = idtable from bill where id = @idBill
	
	declare @count int = 0
	select @count = COUNT(*) from billinfo as bi, bill as b where b.id = bi.idbill and b.id = @idBill and b.statuss = 0
	
	if(@count = 0)
		update tablefood set statuss = N'Trống' where id = @idTable   
end

CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END


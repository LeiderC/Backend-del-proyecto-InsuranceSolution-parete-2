USE [Insurance]
GO

/****** Object: SqlProcedure [dbo].[ManagementPagedList] Script Date: 1/10/2019 9:50:35 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE ManagementPagedList
	@idCustomer int,
    @page int,  
    @rows int 
AS
BEGIN
    declare @tmpManagement table(
		id int not null primary key,
		idCustomer int,
		details nvarchar(400),
		creationDate datetime,
		--endDate datetime,
		userName nvarchar(200),
		idManagementType int,
		managementType nvarchar(50),
		idUser int,
		totalRecords int
    )
	insert into @tmpManagement
	select M.Id, M.IdCustomer, M.Details,M.CreationDate,U.FirstName+ ' '+ u.LastName as userName, M.IdManagementType, MT.Description as managementType, M.IdUser,
	COUNT(*) OVER() totalRecords
	from Management M with(nolock)
	inner join ManagementType MT with(nolock) on M.IdManagementType = MT.Id
	inner join dbo.[User] U with(nolock) on M.IdUser = U.Id
	where EndDate is null and IdCustomer = @idCustomer
	order by CreationDate
	OFFSET (@page - 1)*@rows ROWS                  
    FETCH NEXT @rows ROWS ONLY

	select * from @tmpManagement

	select MT.IdManagement, MT.IdTask, T.Title, T.Description, T.AlertDate, T.ExpirationDate, U.FirstName+ ' '+ u.LastName as userName
	from ManagementTask MT with(nolock)
	inner join @tmpManagement M on MT.IdManagement = M.id
	inner join Task T ON MT.IdTask = T.Id
	inner join [User] U ON T.IdUser = U.Id
	
END

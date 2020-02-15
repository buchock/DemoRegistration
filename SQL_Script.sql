
CREATE TABLE [dbo].[Employee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[Age] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


GO



CREATE PROCEDURE [dbo].[GetAllEmployee]
AS
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT
		Id,
		FirstName,
		LastName,
		Age,
		CreatedDate,
		ModifiedDate
	FROM dbo.Employee;
	
	SET NOCOUNT OFF;
END

GO




CREATE PROCEDURE [dbo].[SaveEmployee]
(
	@strFirstName	VARCHAR(100) = '',
	@strLastName	VARCHAR(100) = '',
	@intAge			INT = 0
)
AS
BEGIN

	DECLARE @ReturnCode		INT,
			@ReturnMessage	VARCHAR(100);
	
	-- 1 - Successulful, -1 Failed
	SET @ReturnCode = 1;
	SET @ReturnMessage = 'Successfully saved the record';
	
	--Set the default value if the variable is NULL
	SET @strFirstName = ISNULL(@strFirstName, '');
	SET @strLastName = ISNULL(@strLastName, '');
	SET @intAge = ISNULL(@intAge, 0);
	
	
	IF @strFirstName = ''
	BEGIN
		SET @ReturnCode = -1;
		SET @ReturnMessage = 'First Name is required';
	END
	ELSE IF @strLastName = ''
	BEGIN
		SET @ReturnCode = -1;
		SET @ReturnMessage = 'Last Name is required';
	END
	ELSE IF @intAge < 18 OR @intAge > 99
	BEGIN
		SET @ReturnCode = -1;
		SET @ReturnMessage = 'Invalid age value. Age should be between 18 and 99';
	END
	ELSE
	BEGIN
		--Save data
		INSERT INTO dbo.Employee(FirstName, LastName, Age, CreatedDate, ModifiedDate)
		SELECT
			@strFirstName,
			@strLastName,
			@intAge,
			GETDATE(),
			NULL
		
		IF @@ERROR > 0
		BEGIN
			SET @ReturnCode = -1;
			SET @ReturnMessage = 'An error is occurred while saving the record';
		END
		
		--If no error, then return the default value of ReturnCode and ReturnMessage
		
	END
	
	SELECT @ReturnCode AS ReturnCode, @ReturnMessage AS ReturnMessage
END

GO




drop table switch

CREATE TABLE dbo.Switch (
	SwitchID int PRIMARY KEY CLUSTERED,
	Colour varchar(50) null,
	IsOn bit not null
); 

insert into switch (SwitchID,Colour,IsOn) values
(1,'Red',0),
(2,'Blue',1)

Select * from switch
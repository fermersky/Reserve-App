create database ReserveClassroomDB2

create table [Groups](
	[Id] int primary key identity,
	[Name] nvarchar(225) not null,
	[StudentsCount] int not null
);

insert into Groups values 
(N'СТ/РПО/17/5 (18:00)', 15),
(N'СТ/РПО/17/2 (18:00)', 15),
(N'ПС/РПО/16/5', 15),
(N'ПС/РПО/16/2', 15),
(N'СТ/КГиД/17/2 (18:00)', 15),
(N'СТ/КГиД/16/5 (18:00)', 15),
(N'СТ/СиКБ/17/5', 15)

create table [Users]( -- пользователи
	[Id] int primary key identity,
	[Login] nvarchar(255) not null,
	[Password] nvarchar(255) not null,
	[Role] nvarchar(25) not null,
	[Fullname] nvarchar(255) not null
);

insert into Users values
('admin', 'admin', 'admin', N'admin'),
('user', 'user', 'user', N'user')

create table [Classrooms]( -- аудиториии
	[Id] int primary key identity,
	[Number] int not null,
	[MaxPersonCount] int not null
);

insert into Classrooms values
(1, 15), (2, 15), (3, 15), (4, 15), (5, 15),
(6, 15), (7, 15), (8, 15), (9, 15), (10, 15),
(11, 15), (12, 15), (13, 15), (14, 15), (15, 15)

create table [Status]
(
	[Id] int primary key identity,
	[Type] nvarchar(30) not null -- Sheduled, Accepted, InProgress (на рассмотрении, все созданные заявки юзером будут получать этот статус)
)
insert into [Status] values
('Sheduled'), ('Accepted'), ('InProgress')

create table [Applications]( -- таблица-оливье
	[Id] int primary key identity,
	[ClassroomId] int foreign key references [Classrooms]([Id]) on delete cascade not null,
	[UserId] int foreign key references Users([Id]) on delete cascade not null, 
	[Date] date not null,
	[LessonNumber] int not null, --[1-8]
	[StudentsCount] int,
	[GroupId] int foreign key references Groups([Id]) on delete cascade not null,
	[StatusId] int foreign key references [Status]([Id]) default 3 not null,
	[Lesson] nvarchar(255) not null, --
	[Comment] nvarchar(255)
);

insert into [Applications] values 
(6, 1, '20190303', 7, 15, 1, 1, N'Net programming', N'Comment'),
(6, 1, '20190303', 8, 15, 1, 1, N'Net programming', N'Comment'),
(3, 2, '20190303', 7, 15, 2, 1, N'WPF', N'Comment'),
(3, 2, '20190303', 8, 15, 2, 1, N'WPF', N'Comment'),
(4, 2, '20190304', 7, 15, 1, 1, N'WPF', N'Comment'),
(4, 2, '20190304', 8, 15, 1, 1, N'WPF', N'Comment'),
(7, 1, '20190304', 1, 15, 2, 1, N'WPF', N'Comment'),
(7, 1, '20190304', 2, 15, 2, 1, N'WPF', N'Comment'),
(4, 2, '20190305', 7, 15, 2, 1, N'WPF', N'Comment'),
(4, 2, '20190305', 8, 15, 2, 1, N'WPF', N'Comment'),
(4, 2, '20190305', 7, 15, 2, 1, N'WPF', N'Comment'),
(4, 2, '20190305', 8, 15, 2, 1, N'WPF', N'Comment')




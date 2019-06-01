create database ReserveClassroomDB

create table [Groups](
	[Id] int primary key identity,
	[Name] nvarchar(225) not null
);

insert into Groups values 
(N'��/���/17/5 (18:00)'),
(N'��/���/17/2 (18:00)'),
(N'��/���/16/5'),
(N'��/���/16/2'),
(N'��/����/17/2 (18:00)'),
(N'��/����/16/5 (18:00)'),
(N'��/����/17/5')

create table [Users]( -- ������� + ������
	[Id] int primary key identity,
	[Login] nvarchar(255) not null,
	[Password] nvarchar(255) not null,
	[Role] nvarchar(25) not null,
	[Fullname] nvarchar(255) not null
);

insert into Users values
('ruban123', 'rubanan', 'User', N'����� ������ �����������'),
('marchenko321', 'tolyan', 'User', N'�������� �������� ���������'),
('gmurmil', '123', 'Admin', N'������� ������ �������������')

create table [Classrooms]( -- ���������
	[Id] int primary key identity,
	[Number] int not null,
	[MaxPersonCount] int not null
);

insert into Classrooms values
(1, 15), (2, 15), (3, 15), (4, 15), (5, 15),
(6, 15), (7, 15), (8, 15), (9, 15), (10, 15),
(11, 15), (12, 15), (13, 15), (14, 15), (15, 15)


create table [Applications]( -- ������ �� ������������
	[Id] int primary key identity,
	[ClassroomId] int foreign key references [Classrooms]([Id]) on delete cascade not null,
	[UserId] int foreign key references Users([Id]) on delete cascade not null, -- �������������
	[Date] date not null,
	[LessonNumber] int not null, -- ����� ���� [1-8]
	[StudentsCount] int,
	[GroupId] int foreign key references Groups([Id]) on delete cascade not null,
	[Lesson] nvarchar(255) not null, -- �������
	[Comment] nvarchar(255)
);

create table [Lessons]( -- ���� �� ���������� + ������������ ������
	[Id] int primary key identity not null,
	[ClassroomId] int foreign key references [Classrooms]([Id]) on delete cascade not null,
	[UserId] int foreign key references Users([Id]) on delete cascade not null,
	[Date] date not null,
	[LessonNumber] int not null, -- ����� ���� [1-8]
	[StudentsCount] int,
	[GroupId] int foreign key references Groups([Id]) on delete cascade not null,
	[Lesson] nvarchar(255) not null, -- �������
	[Comment] nvarchar(255),
	[IsSheduled] bit not null -- ��� ���� �� ��������� ��� ������������
);

select * from Lessons



insert into Lessons values 
-- ���������, ������, ����, ����� ���� [1-8], �-�� ���������, ������, �������, �������, �� ���������� ��� ���
(6, 1, '20190303', 7, 15, 1, N'Net programming', N'Comment', 1),
(6, 1, '20190303', 8, 15, 1, N'Net programming', N'Comment', 1),
(3, 2, '20190303', 7, 15, 2, N'WPF', N'Comment', 1),
(3, 2, '20190303', 8, 15, 2, N'WPF', N'Comment', 1),
(4, 2, '20190304', 7, 15, 1, N'WPF', N'Comment', 1),
(4, 2, '20190304', 8, 15, 1, N'WPF', N'Comment', 1),
(7, 1, '20190304', 1, 15, 2, N'WPF', N'Comment', 1),
(7, 1, '20190304', 2, 15, 2, N'WPF', N'Comment', 1),
(4, 2, '20190305', 7, 15, 2, N'WPF', N'Comment', 1),
(4, 2, '20190305', 8, 15, 2, N'WPF', N'Comment', 1),
(4, 2, '20190305', 7, 15, 2, N'WPF', N'Comment', 1),
(4, 2, '20190305', 8, 15, 2, N'WPF', N'Comment', 1)









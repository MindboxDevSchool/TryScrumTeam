create table Events
(
    Id              uniqueidentifier not null
        constraint Events_pk
            primary key nonclustered,
    CreatedAt       datetime         not null,
    TrackId         uniqueidentifier not null,
    Scale           float,
    Rating          int,
    PhotoUrl        nvarchar(200),
    GeoTagLatitude  float,
    GeoTagLongitude float,
    Comment         nvarchar(200)
)
go

create unique index Events_Id_uindex
    on Events (Id)
go

create table Tracks
(
    Id                    uniqueidentifier not null
        constraint Tracks_pk
            primary key nonclustered,
    Name                  nvarchar(100)    not null,
    CreatedAt             datetime         not null,
    CreatorId             uniqueidentifier not null,
    AllowedCustomizations nvarchar(200)    not null
)
go

create unique index Tracks_Id_uindex
    on Tracks (Id)
go

create table Users
(
    Id             uniqueidentifier not null
        constraint Users_pk
            primary key nonclustered,
    Login          nvarchar(100)    not null,
    HashedPassword nvarchar(550)    not null
)
go

create unique index Users_Id_uindex
    on Users (Id)
go

create unique index Users_Login_uindex
    on Users (Login)
go



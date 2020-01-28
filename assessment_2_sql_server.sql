SELECT 
p.uniqueName as PlatformName, w.id as Id, p.id as PlatformId,
w.uniqueName as UniqueName, w.latitude as Latitude, w.longitude as Longtitude,
w.createdAt as CreatedAt, w.updatedAt as UpdatedAt

FROM 
dbo.platform p 
left join dbo.well w 
on w.platformId = p.id order by w.updatedAt desc;
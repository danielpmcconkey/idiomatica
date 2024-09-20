-- run this on master
Use master
CREATE LOGIN WebAppId FROM EXTERNAL PROVIDER

CREATE USER WebAppId FROM LOGIN WebAppId

SELECT name, type_desc, type, is_disabled 
FROM sys.server_principals
WHERE type_desc like 'external%' 


SELECT roles.principal_id AS RolePID,roles.name AS RolePName,
       server_role_members.member_principal_id AS MemberPID, members.name AS MemberPName
       FROM sys.server_role_members AS server_role_members
       INNER JOIN sys.server_principals AS roles
       ON server_role_members.role_principal_id = roles.principal_id
       INNER JOIN sys.server_principals AS members 
       ON server_role_members.member_principal_id = members.principal_id;



-- run teh rest on the idiomatica DB




SELECT name, type_desc, type 
FROM sys.database_principals 
WHERE type_desc like 'external%'

--ALTER SERVER ROLE ##MS_DefinitionReader## ADD MEMBER WebAppId;
--ALTER SERVER ROLE ##MS_ServerStateReader## ADD MEMBER WebAppId;
--ALTER SERVER ROLE ##MS_ServerStateManager## ADD MEMBER WebAppId;

--DBCC FLUSHAUTHCACHE
--DBCC FREESYSTEMCACHE('TokenAndPermUserStore') WITH NO_INFOMSGS



SELECT   roles.principal_id                          AS RolePrincipalID
    ,    roles.name                                  AS RolePrincipalName
    ,    database_role_members.member_principal_id   AS MemberPrincipalID
    ,    members.name                                AS MemberPrincipalName
FROM sys.database_role_members AS database_role_members  
JOIN sys.database_principals AS roles  
    ON database_role_members.role_principal_id = roles.principal_id  
JOIN sys.database_principals AS members  
    ON database_role_members.member_principal_id = members.principal_id;  
GO

select * from sys.database_principals

ALTER ROLE db_datareader
	ADD MEMBER WebAppId;  
GO
ALTER ROLE db_datawriter
	ADD MEMBER WebAppId;  
GO
db_datareader
db_datawriter
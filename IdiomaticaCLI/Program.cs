﻿// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Model.DAL;
using Logic;
using Model;

using (var context = new IdiomaticaContext())
{
	StatisticsHelper.UpdateAllBookStatsForUserId(context, 1);

	//var path = @"E:\Idiomatica\Idiomatica\Model\Migrations\20240331.08.21_AddHelperColumns.sql";
	//Execute.File(context, path);


}


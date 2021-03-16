using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace StudyTrackr.Database
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}

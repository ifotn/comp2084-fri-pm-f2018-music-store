using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fri_pm_music_store.Models
{
    public class EFAlbums : IAlbumsMock
    {
        // connect to db 
        private MusicStoreModel db = new MusicStoreModel();

        public IQueryable<Album> Albums { get { return db.Albums; } }

        public void Delete(Album album)
        {
            db.Albums.Remove(album);
            db.SaveChanges();
        }

        public Album Save(Album album)
        {
            if (album.AlbumId == 0)
            {
                // insert
                db.Albums.Add(album);
            }
            else
            {
                // update
                db.Entry(album).State = System.Data.Entity.EntityState.Modified;
            }

            db.SaveChanges();
            return album;
        }
    }
}
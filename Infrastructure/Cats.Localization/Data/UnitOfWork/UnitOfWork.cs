﻿using System;
using Cats.Localization.Data.Repository;
using Cats.Localization.Models;


namespace Cats.Localization.Data.UnitOfWork
{
    /// <summary>
    /// UnitOfwork implementation for security module    
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Constructors and private vars

        private readonly TranslationContext _context;
        private IGenericRepository<LocalizedText> localizedTextRepo;
        private IGenericRepository<Language> languageRepo;
        private IGenericRepository<Page> pageRepo;          

        public UnitOfWork()
        {
            this._context = new TranslationContext();            
        }

        #endregion

        public IGenericRepository<LocalizedText> LocalizedTextRepository
        {
            get { return this.localizedTextRepo ?? (this.localizedTextRepo = new GenericRepository<LocalizedText>(_context)); }

        }

        public IGenericRepository<Language> LanguageRepository
        {
            get { return this.languageRepo ?? (this.languageRepo = new GenericRepository<Language>(_context)); }

        }

        public IGenericRepository<Page> PageRepository
        {
            get { return this.pageRepo ?? (this.pageRepo = new GenericRepository<Page>(_context)); }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

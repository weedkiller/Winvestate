using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Winvestate_Offer_Management_Models;
using Winvestate_Offer_Management_Models.Database;
using Winvestate_Offer_Management_Models.Database.Winvestate;

namespace Winvestate_Offer_Management_API.Database
{
    public static class Crud<TEntity> where TEntity : class
    {
        public static bool Delete(TEntity entity)
        {
            using (var conn = Connection.ConnectionWinvestate())
            {
                try
                {
                    return conn.Delete(entity);
                }
                catch (Exception ex)
                {
                    //   Common._graylogger.Error("Problem in delete operation-->" + ex.ToString());
                    //   Common._graylogger.Error("Stack Trace-->" + ex.StackTrace);
                    return false;
                }
            }
        }

        public static dynamic Insert(TEntity entity, out string pException)
        {
            pException = "";
            using (var conn = Connection.ConnectionWinvestate())
            {
                try
                {
                    return conn.Insert(entity);
                }
                catch (Exception ex)
                {
                    pException = ex.Message;
                    // Common._graylogger.Error("Problem in insert operation-->" + ex.ToString());
                    //Common._graylogger.Error("Stack Trace-->" + ex.StackTrace);
                    return -1;
                }
            }
        }

        public static dynamic Insert(List<TEntity> entityList, out string pException)
        {
            pException = "";
            using (var conn = Connection.ConnectionWinvestate())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    foreach (var entity in entityList)
                    {
                        try
                        {
                            conn.Insert(entity, transaction);
                        }
                        catch (Exception ex)
                        {
                            pException = ex.Message;
                            transaction.Rollback();
                            // Common._graylogger.Error("Problem in insert operation-->" + ex.ToString());
                            //Common._graylogger.Error("Stack Trace-->" + ex.StackTrace);
                            return false;
                        }
                    }
                    transaction.Commit();
                }
            }

            return true;
        }

        public static dynamic Delete(List<TEntity> entityList, out string pException)
        {
            pException = "";
            using (var conn = Connection.ConnectionWinvestate())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    foreach (var entity in entityList)
                    {
                        try
                        {
                            conn.Delete(entity, transaction);
                        }
                        catch (Exception ex)
                        {
                            pException = ex.Message;
                            transaction.Rollback();
                            // Common._graylogger.Error("Problem in insert operation-->" + ex.ToString());
                            //Common._graylogger.Error("Stack Trace-->" + ex.StackTrace);
                            return false;
                        }
                    }
                    transaction.Commit();
                }
            }

            return true;
        }

        public static dynamic InsertLog(TEntity entity, out string pException)
        {
            using (var conn = Connection.ConnectionLog())
            {
                pException = "";
                try
                {
                    return conn.Insert(entity);
                }
                catch (Exception ex)
                {
                    pException = ex.Message;
                    // Common._graylogger.Error("Problem in insert operation-->" + ex.ToString());
                    //Common._graylogger.Error("Stack Trace-->" + ex.StackTrace);
                    return -1;
                }
            }
        }

        public static bool Delete(string pQuery)
        {
            using (var conn = Connection.ConnectionWinvestate())
            {
                try
                {
                    return conn.Execute(pQuery) > 0;
                }
                catch (Exception ex)
                {
                    //  Common._graylogger.Error("Problem in update operation-->" + ex.ToString());
                    //  Common._graylogger.Error("Stack Trace-->" + ex.StackTrace);
                    return false;
                }
            }
        }

        public static bool Update(TEntity entity, out string pException)
        {
            using (var conn = Connection.ConnectionWinvestate())
            {
                pException = "";
                try
                {
                    return conn.Update(entity);
                }
                catch (Exception ex)
                {
                    pException = ex.Message;
                    //  Common._graylogger.Error("Problem in update operation-->" + ex.ToString());
                    //  Common._graylogger.Error("Stack Trace-->" + ex.StackTrace);
                    return false;
                }
            }
        }

        public static int InsertAsset(AssetDto pAsset)
        {
            int loResult = 0;
            using var connection = Connection.ConnectionWinvestate();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                Asset loMyAsset = pAsset;
                loResult = (int)connection.Insert(loMyAsset, transaction);
                if (loResult <= 0)
                {
                    transaction.Rollback();
                    return 0;
                }

                pAsset.id = (int)loResult;


                if (pAsset.asset_photos != null && pAsset.asset_photos.Any())
                {
                    foreach (var loAssetPhoto in pAsset.asset_photos)
                    {
                        loAssetPhoto.asset_uuid = loMyAsset.row_guid;
                        if (connection.Insert(loAssetPhoto, transaction) <= 0)
                        {
                            transaction.Rollback();
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return 0;
            }


            transaction.Commit();
            return loResult;
        }

        public static bool UpdateAsset(AssetDto pAsset)
        {

            using var connection = Connection.ConnectionWinvestate();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                Asset loMyAsset = pAsset;
                var loResult = connection.Update(loMyAsset, transaction);
                if (!loResult)
                {
                    transaction.Rollback();
                    return false;
                }

                if (pAsset.asset_photos != null && pAsset.asset_photos.Any())
                {
                    foreach (var loAsset in pAsset.asset_photos)
                    {
                        loAsset.asset_uuid = loMyAsset.row_guid;
                        if (loAsset.id > 0)
                        {
                            if (loAsset.is_deleted)
                            {
                                if (connection.Delete(loAsset, transaction)) continue;
                                transaction.Rollback();
                                return false;
                            }

                            if (connection.Update(loAsset, transaction)) continue;
                            transaction.Rollback();
                            return false;


                        }

                        if (connection.Insert(loAsset, transaction) > 0) continue;
                        transaction.Rollback();
                        return false;

                    }
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
            }


            transaction.Commit();
            return true;
        }

        public static int InsertNewOffer(OfferHistory pOfferHistory, Offer pOffer, Asset pAsset)
        {
            int loResult = 0;
            using var connection = Connection.ConnectionWinvestate();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                loResult = (int)connection.Insert(pOfferHistory, transaction);
                if (loResult <= 0)
                {
                    transaction.Rollback();
                    return 0;
                }

                pOfferHistory.id = (int)loResult;

                if (!connection.Update(pOffer, transaction))
                {
                    transaction.Rollback();
                    return 0;
                }

                if (!connection.Update(pAsset, transaction))
                {
                    transaction.Rollback();
                    return 0;
                }


            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return 0;
            }


            transaction.Commit();
            return loResult;
        }

        public static int InsertCustomerWithOffer(CustomerDto pCustomerDto, Offer pOffer)
        {
            int loResult = 0;
            using var connection = Connection.ConnectionWinvestate();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                Customer loMyCustomer = null;
                if (pCustomerDto.id <= 0)
                {
                    loMyCustomer = pCustomerDto;
                    loResult = (int)connection.Insert(loMyCustomer, transaction);
                    if (loResult <= 0)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                    loMyCustomer.id = (int)loResult;
                }
                else
                {
                    loMyCustomer = pCustomerDto;
                }

                if (pOffer != null)
                {
                    loResult = (int)connection.Insert(pOffer, transaction);
                    if (loResult <= 0)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return 0;
            }


            transaction.Commit();
            return loResult;
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLtoOM.Engine;
using SQLtoOM.Engine.SQLParsers;

namespace SQLtoSQLOMTests {

    [TestClass]
    public class UnitTests_TSqlParser {

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AA_INVALID_QUERY_A() {

            string sql = "SELECT , CMID FROM CMCONTACTS WHERE CMNAME = 'test'";
            string resSql = GetSQL(sql);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AB_INVALID_QUERY_B() {

            string sql = "SELECT CMID FROM CMCONTACTS WHERE CMID = 1'";
            string resSql = GetSQL(sql);
        }

        [TestMethod]
        public void AC_SELECT_ALL_FROM_TABLE() {

            string sql = "SELECT * FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AD_SELECT_ALL_FROM_TABLE_CASESENSITIVE() {

            string sql = "select * from CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql);
        }

        [TestMethod]
        public void AE_SELECT_ONE_COLUMN_FROM_TABLE() {

            string sql = "SELECT CMID FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AF_SELECT_ONE_COLUMN_DISTINCT_FROM_TABLE() {

            string sql = "SELECT DISTINCT CMNAME FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AG_SELECT_TOP_ONE_COLUMN_FROM_TABLE() {

            string sql = "SELECT TOP 2 CMNAME FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AH_SELECT_MANY_COLUMNS_FROM_TABLE() {

            string sql = "SELECT CMID, CMNAME, CMActive FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AI_SELECT_MANY_COLUMNS_AND_CUSTOM_ONES_FROM_TABLE() {

            string sql = "SELECT CMID, CMNAME, 1 A_NUMBER, '2' B_TEXT, CMActive FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AJ_SELECT_ONE_COLUMN_FROM_TABLE_WITH_BASIC_WHERE() {

            string sql = "SELECT CMID FROM CMCONTACTS WHERE CMNAME = 'test'";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AK_SELECT_ONE_COLUMN_FROM_TABLE_WITH_MULTIPLE_WHERE_BOOL() {

            string sql = "SELECT CMID FROM CMCONTACTS WHERE CMNAME = 'test' AND CMACTIVE = 0";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AL_SELECT_ONE_COLUMN_FROM_TABLE_WITH_MULTIPLE_WHERE_BOOL_USING_BOTH_AND_OR() {

            string sql = "SELECT CMID FROM CMCONTACTS WHERE CMNAME = 'test' AND (CMACTIVE = 0 OR CMACTIVE IS NULL)";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AM_SELECT_ONE_COLUMN_FROM_TABLE_WITH_MULTIPLE_WHERE_BOOL_USING_MULTIPLE_AND_OR() {

            string sql = "SELECT CMID FROM CMCONTACTS WHERE (CMFIRSTNAME = 'TEST' OR CMLASTNAME = 'TEST') AND (CMACTIVE = 0 OR CMACTIVE IS NULL)";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AN_SELECT_MANY_COLUMNS_FROM_TABLE_WITH_MULTIPLE_WHERE_BOOL_USING_MULTIPLE_AND_OR() {

            string sql = "SELECT CMID FROM CMCONTACTS WHERE (CMFIRSTNAME = 'TEST' OR CMLASTNAME = 'TEST') AND (CMACTIVE = 0 OR CMACTIVE IS NULL)";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AO_SELECT_ONE_COLUMN_FROM_TABLE_ORDER_BY_COLUMN_DESC() {

            string sql = "SELECT CMNAME FROM CMCONTACTS ORDER BY CMENTRYDATE DESC";
            string resSql = GetSQL(sql);

            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AP_SELECT_CASE_COLUMN_FROM_TABLE() {

            string sql = @"SELECT CASE WHEN CMISACCOUNT = 1 THEN 'IS ACCOUNT'
			               WHEN CMISPERSON = 1 THEN 'IS PERSON'
			               ELSE 'NOTHING' END MYCUSTOMCOLUMN
                           FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AQ_SELECT_CASE_COLUMN_AND_ONE_BASIC_COLUMN_FROM_TABLE_WITH_BASIC_WHERE_BOOL() {

            string sql = @"SELECT CASE WHEN CMISACCOUNT = 1 THEN 'IS ACCOUNT'
			               WHEN CMISPERSON = 1 THEN 'IS PERSON'
			               ELSE 'NOTHING' END MYCUSTOMCOLUMN,
                           CMNAME
                           FROM CMCONTACTS
                           WHERE CMNAME = 'test'";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AR_SELECT_CASE_COLUMN_AND_MANY_COLUMNS_FROM_TABLE_WITH_BASIC_WHERE() {

            string sql = @"SELECT CMID, CASE WHEN CMISACCOUNT = 1 THEN 'IS ACCOUNT'
			               WHEN CMISPERSON = 1 THEN 'IS PERSON'
			               ELSE 'NOTHING' END MYCUSTOMCOLUMN,
                           CMNAME, CMACTIVE
                           FROM CMCONTACTS
                           WHERE CMNAME = 'test' AND CMACTIVE = 0";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AS_SELECT_CASE_COLUMN_AND_MANY_COLUMNS_FROM_TABLE_WITH_BASIC_WHERE_BOOL_USING_MULTIPLE_AND_OR() {

            string sql = @"SELECT CMID, CASE WHEN CMISACCOUNT = 1 THEN 'IS ACCOUNT'
			               WHEN CMISPERSON = 1 THEN 'IS PERSON'
			               ELSE 'NOTHING' END MYCUSTOMCOLUMN,
                           CMNAME, CMACTIVE
                           FROM CMCONTACTS
                           WHERE (CMFIRSTNAME = 'TEST' OR CMLASTNAME = 'TEST') AND (CMACTIVE = 0 OR CMACTIVE IS NULL)";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AT_SELECT_TOP_CASE_COLUMN_AND_MANY_COLUMNS_FROM_TABLE_WITH_BASIC_WHERE_BOOL_USING_MULTIPLE_AND_OR_ORDER_BY_COLUMN_ASC() {
            // Notes: Even though ASC is optional, SqlOm will output it, so we must add it at our input for the unit test to pass
            string sql = @"SELECT TOP 10 CMID, CASE WHEN CMISACCOUNT = 1 THEN 'IS ACCOUNT'
			               WHEN CMISPERSON = 1 THEN 'IS PERSON'
			               ELSE 'NOTHING' END MYCUSTOMCOLUMN,
                           CMNAME, CMACTIVE
                           FROM CMCONTACTS
                           WHERE (CMFIRSTNAME = 'TEST' OR CMLASTNAME = 'TEST') AND (CMACTIVE = 0 OR CMACTIVE IS NULL)
                           ORDER BY CMENTRYDATE ASC";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AU_SELECT_MULTIPLE_COLUMNS_FROM_MANY_TABLES_WITH_JOIN_TABLE_WITHOUT_TABLE_PREFIX() {
            //The following sql should raise a 'Ambiguous column name CMID'. It does not for TSql130Parser....
            string sql = @"SELECT CMID, CMRESOURCES.CMDESCRIPTION
                           FROM CMCONTACTS
                           LEFT JOIN CMRESOURCES ON CMCONTACTS.CMRESOURCEID = CMRESOURCES.CMID";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AV_SELECT_MULTIPLE_COLUMNS_FROM_MANY_TABLES_WITH_JOIN_TABLE_WITH_TABLE_PREFIX() {

            string sql = @"SELECT CMCONTACTS.CMID, CMRESOURCES.CMDESCRIPTION
                           FROM CMCONTACTS
                           LEFT JOIN CMRESOURCES ON CMCONTACTS.CMRESOURCEID = CMRESOURCES.CMID";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AW_SELECT_MULTIPLE_COLUMNS_FROM_MANY_TABLES_WITH_JOIN_TABLE_WITH_PARENTHESES_IN_JOIN() {

            string sql = @"SELECT CMCONTACTS.CMID, CMRESOURCES.CMDESCRIPTION
                           FROM CMCONTACTS
                           LEFT JOIN CMRESOURCES ON (CMCONTACTS.CMRESOURCEID = CMRESOURCES.CMID)";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AX_SELECT_TOP_MULTIPLE_COLUMNS_WITH_CASE_FROM_MANY_TABLES_WITH_JOIN_TABLE_AND_MULTIPLE_WHERE_BOOL_AND_ORDER_BY() {

            string sql = @"SELECT TOP 10 
                           CMCONTACTS.CMID, 
                           CASE WHEN CMCONTACTS.CMISACCOUNT = 1 THEN 'IS ACCOUNT'
			               WHEN CMCONTACTS.CMISPERSON = 1 THEN 'IS PERSON'
			               ELSE 'NOTHING' END MYCUSTOMCOLUMN,
                           CMCONTACTS.CMNAME, 
                           CMRESOURCES.CMACTIVE,
                           CMRESOURCES.CMDESCRIPTION
                           FROM CMCONTACTS
                           INNER JOIN CMRESOURCES ON (CMCONTACTS.CMRESOURCEID = CMRESOURCES.CMID)
                           WHERE (CMCONTACTS.CMFIRSTNAME = 'TEST' OR CMCONTACTS.CMLASTNAME = 'TEST') AND (CMRESOURCES.CMACTIVE = 0 OR CMRESOURCES.CMACTIVE IS NULL)
                           ORDER BY CMCONTACTS.CMENTRYDATE ASC";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AY_SELECT_STAR_COLUMNS_WITH_TABLE_PREFIX_FROM_MANY_TABLES_WITH_JOIN_TABLE() {

            string sql = @"SELECT CMCONTACTS.*, 
                           CMRESOURCES.*
                           FROM CMCONTACTS
                           INNER JOIN CMRESOURCES ON CMCONTACTS.CMRESOURCEID = CMRESOURCES.CMID";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void AZ_SELECT_MULTIPLE_COLUMNS_WITH_AS_ALIAS_FROM_TABLES_WITH_ALIAS() {
            // SqlOm omits the 'AS' alias for tables.. Thus, we exclude it for the test to pass.
            string sql = @"SELECT CMCONTACTS.CMNAME CONTACT, 
                           CMRESOURCES.CMDESCRIPTION RESOURCE
                           FROM CMCONTACTS CONT
                           INNER JOIN CMRESOURCES RES ON CONT.CMRESOURCEID = RES.CMID";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BA_SELECT_MULTIPLE_COLUMNS_FROM_MANY_TABLES_WITH_JOIN_MULTIPLE_TABLES() {

            string sql = @"SELECT CMRELATEDCONTACTS.CMNAME RELATEDCONT_NAME, CMCONTACTS.CMID, CMRESOURCES.CMDESCRIPTION, GXUSER.GXLOGINNAME
                           FROM CMCONTACTS
                           INNER JOIN CMRELATEDCONTACTS rel ON rel.CMRELCONTID = CMCONTACTS.CMID
                           LEFT JOIN CMRESOURCES ON CMCONTACTS.CMRESOURCEID = CMRESOURCES.CMID
                           LEFT JOIN GXUSER ON CMRESOURCES.CMUSERID = GXUSER.GXID";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BB_SELECT_TOP_MULTIPLE_COLUMNS_WITH_CUSTOM_COLUMNS_AND_COMPLEX_CASE_USING_MANY_TABLES_JOINED_WITH_MULTIPLE_WHERE_AND_ORDER_BY() {

            string sql = @"SELECT TOP 10
                           CMRELATEDCONTACTS.CMNAME RELATEDCONT_NAME, 
                           CMCONTACTS.CMID, 
                           CMRESOURCES.CMDESCRIPTION, 
                           1 A, 
                           '2' B,
                           GXUSER.GXLOGINNAME,
                           CASE WHEN CMISACCOUNT = 1 AND CMISPERSON = 0 THEN 'ACCOUNT'
			               WHEN CMISACCOUNT = 0 AND CMISPERSON = 1 THEN 'PERSON'
                           WHEN CMISCOMPANY = 1 THEN 'COMPANY'
			               ELSE 'UNKNOWN' END CONTACT_TYPE
                           FROM CMCONTACTS
                           INNER JOIN CMRELATEDCONTACTS REL ON REL.CMRELCONTID = CMCONTACTS.CMID
                           LEFT JOIN CMRESOURCES ON CMCONTACTS.CMRESOURCEID = CMRESOURCES.CMID
                           LEFT JOIN GXUSER ON CMRESOURCES.CMUSERID = GXUSER.GXID
                           WHERE 1 = 1 AND CMRESOURCES.CMDESCRIPTION = 'XXX'
                           ORDER BY CMCONTACTS.CMNAME ASC";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BC_SELECT_COLUMN_GROUP_BY_ONE_COLUMN() {

            string sql = @"SELECT 
                           CMNAME
                           FROM CMCONTACTS
                           GROUP BY CMNAME";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BD_SELECT_COLUMN_GROUP_BY_ONE_COLUMN_WITH_TABLE_ALIAS() {

            string sql = @"SELECT 
                           CMNAME
                           FROM CMCONTACTS
                           GROUP BY CMCONTACTS.CMNAME";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BE_SELECT_COLUMN_GROUP_BY_MULTIPLE_COLUMNS() {

            string sql = @"SELECT 
                           CMNAME
                           FROM CMCONTACTS
                           GROUP BY CMCONTACTS.CMNAME, CMACTIVE";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BF_SELECT_COLUMN_USING_PARAMETERS_AT_WHERE() {

            string sql = @"SELECT 
                           CMNAME
                           FROM CMCONTACTS
                           WHERE CMNAME = @name";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BG_SELECT_COLUMN_USING_FUNCTION_COUNT() {

            string sql = @"SELECT COUNT(*) TIMES_FOUND,
                           CMFIRSTNAME
                           FROM CMCONTACTS
                           WHERE CMNAME LIKE 'MICHAEL%'
                           GROUP BY CMFIRSTNAME";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BH_SELECT_COLUMN_USING_FUNCTION_SUM() {

            string sql = @"SELECT SUM(CMREVNUM) col_sum,
                           CMFIRSTNAME
                           FROM CMCONTACTS
                           WHERE CMNAME LIKE 'MICHAEL%'
                           GROUP BY CMFIRSTNAME";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BI_SELECT_COLUMN_USING_FUNCTION_MAX() {

            string sql = @"SELECT MAX(CMREVNUM) max_revnum,
                           CMFIRSTNAME
                           FROM CMCONTACTS
                           WHERE CMNAME LIKE 'MICHAEL%'
                           GROUP BY CMFIRSTNAME";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BJ_SELECT_COLUMN_USING_MULTIPLE_FUNCTIONS() {

            string sql = @"SELECT 
                           CMFIRSTNAME,
                           COUNT(1) count_no,
                           AVG(CMREVNUM) AVG_REVNUM,
                           SUM(CMREVNUM) TOTAL_REVNUM
                           FROM CMCONTACTS
                           GROUP BY CMFIRSTNAME";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BK_SELECT_WITH_SUBQUERY_IN_COLUMN() {

            string sql = @"SELECT CMNAME, 
                           (SELECT TOP 1 CMTEXT CONT_MESSAGE
                           FROM CMCONTACTMESSAGES WHERE CMCONTACTS.CMID = CMCONTACTMESSAGES.CMCONTACTID ORDER BY CMPOSTDATE DESC) CONTACT_MESSAGE
                           FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BL_SELECT_WITH_NOT_EXISTS_SUBQUERY_IN_WHERE() {

            string sql = @"SELECT CMNAME
                           FROM CMCONTACTS
                           WHERE NOT EXISTS (SELECT CMID 
                           FROM CMCONTACTMESSAGES WHERE CMCONTACTS.CMID = CMCONTACTMESSAGES.CMCONTACTID)";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BM_SELECT_WITH_EXISTS_SUBQUERY_IN_WHERE() {

            string sql = @"SELECT CMNAME
                           FROM CMCONTACTS
                           WHERE EXISTS (SELECT CMID 
                           FROM CMCONTACTMESSAGES WHERE CMCONTACTS.CMID = CMCONTACTMESSAGES.CMCONTACTID)";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BN_SELECT_WHERE_USING_IN_VALUES() {

            string sql = @"SELECT CMNAME
                           FROM CMCONTACTS
                           WHERE CMID IN ('id1', 'id2', 'id3')";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BO_SELECT_WHERE_USING_IN_SUBQUERY() {

            string sql = @"SELECT CMNAME
                           FROM CMCONTACTS
                           WHERE CMID IN (SELECT CMID 
                           FROM CMCONTACTMESSAGES WHERE CMCONTACTS.CMID = CMCONTACTMESSAGES.CMCONTACTID)";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BP_TEST_WHERE_ORDER() {

            string sql = @"SELECT * FROM CMCONTACTS
                           WHERE 
                           CMOTHERID IN ('1')
                           OR CMCONTACTS.CMID IN ('id1', 'id2', 'id3')
                           AND (1 = 1 OR CMCONTACTS.CMACTIVE = 1)";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BQ_TEST_WHERE_ORDER() {

            string sql = @"SELECT *
                            FROM CMCONTACTS WHERE 
                            CMCONTACTS.CMID = '1'
                            and EXISTS (SELECT CMID FROM CMCONTACTMESSAGES WHERE CMCONTACTS.CMID = CMCONTACTMESSAGES.CMCONTACTID)
                            OR CMCONTACTS.CMACTIVE = @contActive AND GXUSER.GXACTIVE = 1";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BR_TEST_WHERE_ORDER() {
            //Using no parentheses, generates a different outcome than expected
            string sql = @"SELECT *
                            FROM CMCONTACTS WHERE 
                            CMCONTACTS.CMID = '1' 
                            AND CMCONTACTS.CMACTIVE = 0
                            OR CMCONTACTS.CMLASTNAME = 'TEST' 
                            OR CMCONTACTS.FIRSTNAME = 'TEST'";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreNotEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BS_TEST_WHERE_ORDER() {
            //Using parentheses, generates the desired outcome
            string sql = @"SELECT *
                            FROM CMCONTACTS WHERE 
                            (CMCONTACTS.CMID = '1' OR CMCONTACTS.CMACTIVE = 0) OR 
                            (CMCONTACTS.CMLASTNAME = 'TEST' AND CMCONTACTS.FIRSTNAME = 'TEST')";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BT_SELECT_HAVING() {
            string sql = @"SELECT CMNAME, SUM(CMREVNUM)  TOTAL_NUM
                           FROM CMCONTACTS
                           GROUP BY CMNAME
                           HAVING SUM(CMREVNUM) > 3";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BU_LATEST_FORM_QUERY() {
            string sql = @"SELECT TOP 10
                            REL.CMID RELATEDCONT_ID, 
                            CMCONTACTS.CMID, 
                            CMRESOURCES.CMDESCRIPTION, 
                            1 A, 
                            '2' B,
                            GXUSER.GXLOGINNAME,
                            COUNT(1) COUNT_NO,
                            AVG(CMREVNUM) AVG_REVNUM,
                            SUM(CMREVNUM) TOTAL_REVNUM,
                            CASE WHEN CMISACCOUNT = 1 AND CMISPERSON = 0 THEN 'ACCOUNT'
                            WHEN CMISACCOUNT = 0 AND CMISPERSON = 1 THEN 'PERSON'
                            WHEN CMISCOMPANY = 1 THEN 'COMPANY'
                            ELSE 'UNKNOWN' END CONTACT_TYPE,
                            (SELECT TOP 1 CMTEXT CONT_MESSAGE
                            FROM CMCONTACTMESSAGES WHERE CMCONTACTS.CMID = CMCONTACTMESSAGES.CMCONTACTID AND CMCONTACTMESSAGES.CMWHATEVER = @subQryVal ORDER BY CMPOSTDATE DESC) CONTACT_MESSAGE
                            FROM CMCONTACTS
                            INNER JOIN CMCONTACTRELATIONS REL ON (REL.CMRELCONTID = CMCONTACTS.CMID OR REL.CMRELCONTID IS NULL)
                            LEFT JOIN CMRESOURCES ON CMCONTACTS.CMRESOURCEID = CMRESOURCES.CMID
                            LEFT JOIN GXUSER ON CMRESOURCES.CMUSERID = GXUSER.GXID
                            WHERE 
                            (CMCONTACTS.CMID IN ('id1', 'id2', 'id3')
                            AND EXISTS (SELECT CMID FROM CMCONTACTMESSAGES WHERE CMCONTACTS.CMID = CMCONTACTMESSAGES.CMCONTACTID))
                            OR (CMCONTACTS.CMACTIVE = @contActive AND 
                            (CMRESOURCES.CMDESCRIPTION = 'test' AND GXUSER.GXACTIVE = 1)) 
                            GROUP BY 
                            CMRESOURCES.CMACTIVE, 
                            CMCONTACTS.CMNAME, 
                            REL.CMID, 
                            CMCONTACTS.CMID, 
                            CMRESOURCES.CMDESCRIPTION, 
                            GXUSER.GXLOGINNAME, 
                            CMISACCOUNT, 
                            CMISPERSON, 
                            CMISCOMPANY
                            HAVING SUM(CMCONTACTS.CMREVNUM) > 1
                            ORDER BY 
                            CMCONTACTS.CMNAME ASC";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BV_PPAP() {
            string sql = @"select max(test_col) from lalalo";
            string resSql = GetSQL(sql);
        }

        [TestMethod]
        public void BW_SELECT_WITH_COALESCE() {
            string sql = @"SELECT COALESCE(CMREFERENCENUM, 'UNKNOWN') CUSTOM_REFNUM FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BX_SELECT_FROM_SUBQUERY() {
            string sql = @"SELECT * FROM (SELECT CMID FROM AAAA) X";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BY_SELECT_CASE_COLUMN_FROM_TABLE_WITH_ELSE_NULL() {

            string sql = @"SELECT CASE WHEN CMISACCOUNT = 1 THEN 'IS ACCOUNT'
			               WHEN CMISPERSON = 1 THEN 'IS PERSON'
			               ELSE NULL END MYCUSTOMCOLUMN
                           FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void BZ_SELECT_CASE_COLUMN_WITH_SUBCASE() {

            string sql = @"SELECT
	                         CASE
		                        WHEN CMISACCOUNT = 1 THEN 'ACCOUNT'
		                        WHEN CMISCOMPANY = 1 THEN 
                                   CASE
				                      WHEN CMREVNUM = 1 THEN 'Other'
				                      ELSE NULL
			                       END
		                        ELSE 'UNKNOWN'
	                         END CONTACT_TYPE
                           FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void CA_SELECT_FROM_TABLE_WITH_JOIN_FROM_QUERY() {

            string sql = @"SELECT CMNAME FROM CMCONTACTS C
                           INNER JOIN (SELECT CMID, CMDESCRIPTION FROM CMRESOURCES) CUST_RESOURCES ON C.CMRESOURCEID = CUST_RESOURCES.CMID";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void CB_SELECT_FROM_TABLE_WITH_JOIN_FROM_QUERY_USING_PARENTHESES() {

            string sql = @"SELECT CMNAME FROM CMCONTACTS C
                           INNER JOIN (SELECT CMID, CMDESCRIPTION FROM CMRESOURCES) R ON (C.CMRESOURCEID = R.CMID)";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void CB_SELECT_COLUMN_WITH_MATH_OPERATIONS() {

            string sql = @"SELECT CMREVNUM * -1 CALC_REV FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void CC_SELECT_COLUMN_WITH_MULTIPLE_MATH_OPERATIONS() {

            string sql = @"SELECT (CMREVNUM * -1) / 2 CALC_REV FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void CD_SELECT_COLUMN_WITH_MULTIPLE_MATH_OPERATIONS_2() {

            string sql = @"SELECT CMCONTACTS.CMREVNUM ORIGINAL_VAL, ((2 + COALESCE(CMCONTACTS.CMREVNUM, 0)) / 2) CALC_VAL FROM CMCONTACTS";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        [TestMethod]
        public void CE_ARETI_01() {

            string sql = @"SELECT 
                            (CASE 
	                            WHEN TENT.GXSTATUS = 3 THEN (-1) * TRTR.GXSIGNLDEBIT * TERL.GXLAMOUNT 
	                            ELSE TRTR.GXSIGNLDEBIT * TERL.GXLAMOUNT 
                            END) DEBIT 
                            FROM GXTRADEENTRYRCNCL TERL";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }


        [TestMethod]
        public void CF_NIAXOS_01() {

            string sql = @"SELECT TRDS.GXPOSTALCODE Zip,
                            COALESCE(TRDS.GXSTREET, '') + ' ' + COALESCE(TRDS.GXSTREETNUMBER, '') Address
                            FROM GXCOMMERCIALENTRY CENT
                            LEFT JOIN GXTRADERSITE TRDS ON TRDS.GXID = CENT.GXSHIPTOTRDSID
                            WHERE TENT.GXID = @TENTID";
            string resSql = GetSQL(sql);

            AutoFixOutputSqlForCompare(ref sql, ref resSql);
            Assert.AreEqual(sql, resSql, true);
        }

        #region Helper methods
        private void AutoFixOutputSqlForCompare(ref string sql, ref string resSql) {
            RemoveParentheses(ref sql);
            RemoveParentheses(ref resSql);
            MakeSqlOmValidOutput(ref sql);
            MakeSqlOmValidOutput(ref resSql);
        }

        private string GetSQL(string sql) {
            SqlOMEngine engine = new SqlOMEngine();
            engine.ParseSQL(RendererType.MsSQL, sql);
            string resSql = engine.GetGeneratedSql();

            RemoveBrackets(ref resSql);

            return resSql;
        }

        private void RemoveBrackets(ref string sql) {
            sql = sql
                .Replace("[", "")
                .Replace("]", "");
        }

        private void RemoveParentheses(ref string sql) {
            sql = sql
                .Replace("(", "")
                .Replace(")", "");
        }

        private void MakeSqlOmValidOutput(ref string sql) {
            sql = sql
               .Replace("  ", " ")
               .Replace(" ,", ",")
               .Replace("\r", "")
               .Replace("\n", "")
               .Replace("\t", "")
               .Trim();

            sql = string.Join(" ", sql.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }
        #endregion
    }
}

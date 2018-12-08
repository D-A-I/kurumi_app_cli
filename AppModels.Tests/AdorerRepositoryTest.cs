using System;
using System.Linq;
using AppModels.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppModels.Tests {
    public class AdorerRepositoryTest {
        /**
         * ### 以下参照 ###
         * https://docs.microsoft.com/ja-jp/ef/core/miscellaneous/testing/in-memory
         * https://msdn.microsoft.com/ja-jp/magazine/mt797648.aspx
         */
        [Fact(DisplayName = "Add()の正常系テスト。2テーブルに1件ずつデータが登録される")]
        public void Test01_Add_01() {
            // テストメソッド毎に一意のInMemoryDBを用意する
            var options = new DbContextOptionsBuilder<kurumi.kurumiContext>()
                .UseInMemoryDatabase(databaseName: "Test01_GetAll_01").Options;

            // Repositoryからテストデータを追加
            using(var context = new kurumi.kurumiContext(options)) {
                var repo = new AdorerRepository(context);
                var data = new kurumi.TaskOfGroup {
                    GroupId = 1,
                    GroupName = "TEST_GROUP",
                    TaskId = 2,
                    Status = 3,
                    Content = "TEST_CONTENT",
                    Pic = "TEST_PIC",
                    Period = 99991231
                };
                repo.Add(data);
            }

            // 異なるcontextから（直接）テストデータが登録されていることを確認する
            using(var context = new kurumi.kurumiContext(options)) {
                // TaskGroupの確認
                Assert.Equal(1, context.TaskGroup.FirstOrDefault().GroupId);
                Assert.Equal("TEST_GROUP", context.TaskGroup.FirstOrDefault().Name);
                // Tasksの確認
                Assert.Equal(1, context.Tasks.FirstOrDefault().GroupId);
                Assert.Equal(2, context.Tasks.FirstOrDefault().TaskId);
                Assert.Equal(3, context.Tasks.FirstOrDefault().Status);
                Assert.Equal("TEST_CONTENT", context.Tasks.FirstOrDefault().Content);
                Assert.Equal("TEST_PIC", context.Tasks.FirstOrDefault().Pic);
                Assert.Equal(99991231, context.Tasks.FirstOrDefault().Period);
            }
        }

        [Fact(DisplayName = "GetAll()の正常系テスト")]
        public void Test02_GetAll_01() {
            // 一意のInMemoryDB
            var options = new DbContextOptionsBuilder<kurumi.kurumiContext>()
                .UseInMemoryDatabase(databaseName: "Test02_GetAll_01").Options;

            // contextからテストデータを追加
            using(var context = new kurumi.kurumiContext(options)) {
                var data1 = new kurumi.TaskGroup() {
                    GroupId = 9,
                    Name = "TEST_GROUP"
                };
                context.TaskGroup.Add(data1);
                var data2 = new kurumi.Tasks() {
                    GroupId = 9,
                    TaskId = 8,
                    Status = 7,
                    Content = "TEST_CONTENT",
                    Pic = "TEST_PIC",
                    Period = 19000101
                };
                context.Tasks.Add(data2);
                context.SaveChanges();
            }
            
            // GetAllの確認
            using(var context = new kurumi.kurumiContext(options)) {
                var repo = new AdorerRepository(context);
                var result = repo.GetAll().ToList();
                // マッチングキーの確認
                Assert.Single(result);
                // 各項目の確認
                Assert.Equal(9, result.FirstOrDefault().GroupId);
                Assert.Equal("TEST_GROUP", result.FirstOrDefault().GroupName);
                Assert.Equal(8, result.FirstOrDefault().TaskId);
                Assert.Equal(7, result.FirstOrDefault().Status);
                Assert.Equal("TEST_CONTENT", result.FirstOrDefault().Content);
                Assert.Equal("TEST_PIC", result.FirstOrDefault().Pic);
                Assert.Equal(19000101, result.FirstOrDefault().Period);
            }
        }
    }
}
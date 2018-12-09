using System;
using System.Collections.Generic;
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
        [Fact(DisplayName = "AdorerRepository#Addの正常系テスト。2テーブルに1件ずつデータが登録される")]
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
                    Period = 19000101
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
                Assert.Equal(19000101, context.Tasks.FirstOrDefault().Period);
            }
        }

        [Fact(DisplayName = "AdorerRepository#GetAllの正常系テスト")]
        public void Test02_GetAll_01() {
            // 一意のInMemoryDB
            var options = new DbContextOptionsBuilder<kurumi.kurumiContext>()
                .UseInMemoryDatabase(databaseName: "Test02_GetAll_01").Options;

            // contextからテストデータを追加
            using(var context = new kurumi.kurumiContext(options)) {
                var data1 = new kurumi.TaskGroup() {
                    GroupId = 1,
                    Name = "TEST_GROUP"
                };
                context.TaskGroup.Add(data1);
                var data2 = new kurumi.Tasks() {
                    GroupId = 1,
                    TaskId = 2,
                    Status = 3,
                    Content = "TEST_CONTENT",
                    Pic = "TEST_PIC",
                    Period = 19000101
                };
                context.Tasks.Add(data2);
                context.SaveChanges();
            }

            // 確認
            using(var context = new kurumi.kurumiContext(options)) {
                var repo = new AdorerRepository(context);
                var result = repo.GetAll().ToList();
                // マッチングキーの確認
                Assert.Single(result);
                // 各項目の確認
                Assert.Equal(1, result.FirstOrDefault().GroupId);
                Assert.Equal("TEST_GROUP", result.FirstOrDefault().GroupName);
                Assert.Equal(2, result.FirstOrDefault().TaskId);
                Assert.Equal(3, result.FirstOrDefault().Status);
                Assert.Equal("TEST_CONTENT", result.FirstOrDefault().Content);
                Assert.Equal("TEST_PIC", result.FirstOrDefault().Pic);
                Assert.Equal(19000101, result.FirstOrDefault().Period);
            }
        }

        [Fact(DisplayName = "AdorerRepository#GetByGroupIdの正常系テスト。keyによる絞り込み確認")]
        public void Test03_GetByGroupId_01() {
            // 一意のInMemoryDB
            var options = new DbContextOptionsBuilder<kurumi.kurumiContext>()
                .UseInMemoryDatabase(databaseName: "Test03_GetByGroupId_01").Options;

            // contextからテストデータを追加
            using(var context = new kurumi.kurumiContext(options)) {
                var groupList = new List<kurumi.TaskGroup>(){
                    new kurumi.TaskGroup() {
                        GroupId = 1,
                        Name = "TEST_GROUP1"
                    },
                    new kurumi.TaskGroup() {
                        GroupId = 2,
                        Name = "TEST_GROUP2"
                    }
                };
                context.TaskGroup.AddRange(groupList);
                var taskList = new List<kurumi.Tasks>() {
                    new kurumi.Tasks() {
                        GroupId = 1,
                        TaskId = 2,
                        Status = 3,
                        Content = "TEST_CONTENT",
                        Pic = "TEST_PIC",
                        Period = 19000101
                    },
                    new kurumi.Tasks() {
                        GroupId = 2,
                        TaskId = 99,
                        Status = 99,
                        Content = "TEST_CONTENT",
                        Pic = "TEST_PIC",
                        Period = 99991231
                    }
                };
                context.Tasks.AddRange(taskList);
                context.SaveChanges();
            }

            // 確認
            using(var context = new kurumi.kurumiContext(options)) {
                var repo = new AdorerRepository(context);
                var result = repo.GetByGroupId(1).ToList();
                // マッチングキーの確認
                Assert.Single(result);
            }
        }
    }
}
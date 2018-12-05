using System;
using System.Linq;
using AppModels.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppModels.Tests {
    public class AdorerRepositoryTest {
        [Fact(DisplayName="Addの正常系テスト。2テーブルに1件ずつデータを登録する")]
        public void Test01_Add_01() {
            // テストメソッド毎に一意のInMemoryDBを用意する
            var options = new DbContextOptionsBuilder<kurumi.kurumiContext>()
                .UseInMemoryDatabase(databaseName: "Test01_GetAll_01").Options;

            // テストデータ追加
            using(var context = new kurumi.kurumiContext(options)) {
                var repo = new AdorerRepository(context);
                var data = new kurumi.TaskOfGroup {
                    GroupId = 1,
                    GroupName = "TEST_GNAME",
                    TaskId = 1,
                    Status = 0,
                    Content = "TEST_CONTENT",
                    Pic = "TEST_PIC",
                    Period = 99991231
                };
                repo.Add(data);
            }

            // 異なるcontextで、テストデータが保存されていることを確認する
            using(var context = new kurumi.kurumiContext(options)) {
                Assert.Null(context.TaskGroup.ToList());
                Assert.Single(context.Tasks.ToList());
            }
        }
    }
}
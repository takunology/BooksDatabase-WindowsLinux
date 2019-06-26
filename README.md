# BooksDatabase(Windows, Linux 版) - 文献管理システム
## 1. このWebアプリについて
参考文献を管理(追加や編集)しておくことができるWebアプリです。
ログインしていない状態では文献の閲覧、ログインすると文献の追加や編集などができます。
<br />
<br />

## 2. CentOS7 にて Apacheを立ち上げて外部から接続する方法
以下のような環境を用意してください。MariaDBは初期設定 (文字コードは utf8) を済ませてください。
- CentOS7 (1810)
- MariaDB - Windows版（MySQL）10.3.16
- .NET Core 2.2.300
- Apache 2.4.6 (CentOS)

NuGet で MySQL 接続ツール (Pomelo.EntityFrameworkCore.MySql) を導入してください。バージョンは 2.1.0 です。<br/>
また、dotnet コマンドを使用できるようにしておいてください。セットアップ例は以下の通りです。
```
sudo rpm -Uvh https://packages.microsoft.com/config/rhel/7/packages-microsoft-prod.rpm
sudo yum update
sudo yum install dotnet-sdk-2.2
```

リポジトリをクローンしたら Booksdatabase/Booksdatabase ディレクトリに移動します。そして、リリース版にデプロイします。

```
dotnet publish --configuration Release
```
<br/>

## 2.1 データベースの更新
できたらデータベースを更新します。まずは文献管理用のデータベースです。

```
dotnet ef database update --context BooksDbContext
```

続いて、ユーザ認証用のデータベースを更新します。

```
dotnet ef database update --context ApplicationDbContext
```

ここで、データベースの設定を変更します。
```
MariaDB [(none)]> show databases;

+----------------------+
| Database             |
+----------------------+
| information_schema   |
| ApplicationDbContext |
| BooksDataBaseContext |
| mysql                |
| performance_schema   |
+----------------------+
```
この中の **BooksDataBaseContext** を選択し、
```
MariaDB [(none)]> use ApplicationDbContext

MariaDB [ApplicationDbContext]> show tables;
+--------------------------------+
| Tables_in_BooksDataBaseContext |
+--------------------------------+
| Books                          |
| __EFMigrationsHistory          |
+--------------------------------+
2 rows in set (0.00 sec)

```
ここで次のコマンドを入力します。
```
ALTER TABLE `Books` CHANGE COLUMN `Id` `Id` INT(11) NOT NULL AUTO_INCREMENT FIRST;
```
デフォルトのままではキーが生成されず、文献を登録するとエラーが起きてしまいます。これを自動で割り振るように設定する必要があります。これでデータベースの設定は以上です。

<br/>

## 2.2 Apacheの設定とアプリの起動
次に、Apache にリバースプロキシを設定します。設定ファイル : /etc/httpd/conf/httpd.conf 

```conf
#<Directory />
 #   AllowOverride none
 #   Require all denied
#</Directory>
ProxyPreserveHost On
ProxyPass / http://0.0.0.0:8000/
ProxyPassReverse / http://0.0.0.0:8000/
```

次に、ファイアウォール系を無効にします。

```
sudo systemctl disable firewalld
```
SELinux も無効化してください。できたらシステムを再起動して設定を反映します。 <br/>
アプリの実行は Booksdatabase/Booksdatabase ディレクトリにて実行します。

```
sudo dotnet run
```

これでサーバが起動するので、ホスト側からゲスト側のIPアドレスにアクセスしてみてください。

<br />
<br />

## 3. Windows10 (ローカル) での実行方法
次のような環境を用意してください。Windows 版の MariaDB を導入すると、GUIアプリがインストールされます。
- MariaDB（MySQL）10.3.16
- .NET Core 2.1.700 (VisualStudio から導入できます)

リポジトリをクローン出来たらエクスプローラから BooksDatabase/BooksDatabase ディレクトリにて powershell を起動します。

<br/>

## 3.1 データベースの更新
powershell からデータベースを更新します。文献管理データベースは

```
dotnet ef database update --context BooksDbContext
```

ユーザ認証用データベースは

```
dotnet ef database update --context ApplicationDbContext
```

を入力します。ここで、データベースの設定を行います。HeidiSQL アプリから作成したデータベースへログインします。 booksdbcontext を選択すると、画面右に books が表示されるのでこれを開きます。ID の行の右のほうに Default という欄があるのでこれをクリックして　AUTO_INCREMENT を選択してセーブします。<br />
これでデータベースの設定は完了です。

<br/>

## 3.2 アプリの起動
powershell から　BooksDatabase/BooksDatabase ディレクトリにて

```
dotnet run
```
を実行すると、サーバが立ちあがります。あとはブラウザを使って自身の PC の IP アドレス先にアクセスしてください。セキュリティ対策が甘いので、ローカルで使用するようお願いします。

<br />

## 4. 動作している様子
CentOS7 でサーバを立て、そこにアクセスしている様子です。
<a href="http://f.hatena.ne.jp/takunology/20190624003814"><img src="https://cdn-ak.f.st-hatena.com/images/fotolife/t/takunology/20190624/20190624003814.png" alt="20190624003814"></a>

<br/>
アプリの製作背景については https://blog.takunology.jp/entry/2019/06/20/034726 にて紹介しています。

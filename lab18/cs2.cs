using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace lab18
{
    [Serializable]
    class Album
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public int Year { get; set; }
        public double Duration { get; set; }
        public string RecordingStudio { get; set; }
        public List<Song> Songs { get; set; }

        public Album()
        {
            Songs = new List<Song>();
        }

        public override string ToString()
        {
            string albumInfo = $"Title: {Title}\nArtist: {Artist}\nYear: {Year}\nDuration: {Duration}\nRecording Studio: {RecordingStudio}";

            string songsInfo = "Songs:\n";
            foreach (var song in Songs)
            {
                songsInfo += song + "\n";
            }

            return albumInfo + "\n" + songsInfo;
        }
    }

    [Serializable]
    class Song
    {
        public string Title { get; set; }
        public double Duration { get; set; }
        public string Genre { get; set; }

        public override string ToString()
        {
            return $"Title: {Title}\nDuration: {Duration}\nGenre: {Genre}";
        }
    }

    internal class cs2
    {
        static List<Album> albums = new List<Album>();
        public static void task_2()
        {
            Console.OutputEncoding = Encoding.Unicode;
            
            while (true)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Створення альбому");
                Console.WriteLine("2. Виведення інформації про альбоми");
                Console.WriteLine("3. Додавання пісні до альбому");
                Console.WriteLine("4. Серіалізація альбому");
                Console.WriteLine("5. Збереження серіалізованих альбомів у файл");
                Console.WriteLine("6. Завантаження серіалізованих альбомів з файлу");
                Console.WriteLine("7. Вихід");
                Console.WriteLine("Виберіть опцію:");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        //Введення даних про альбом
                        CreateAlbum();
                        break;
                    case "2":
                        //Виведення інформації про альбом 
                        DisplayAlbums();
                        break;
                    case "3":
                        //Введення даних про пісню
                        AddSongToAlbum();
                        break;
                    case "4":
                        //Серилізація альбому
                        SerializeAlbum();
                        break;
                    case "5":
                        //Збереження серелізованогу альбому у файл
                        SaveSerializedAlbumsToFile();
                        break;
                    case "6":
                        //Завантаження серелізованого альбому
                        LoadSerializedAlbumsFromFile();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }

                Console.WriteLine();
            }

        }
        static void CreateAlbum()
        {
            Console.WriteLine("Створення альбому");

            Console.Write("Назва альбому: ");
            string title = Console.ReadLine();

            Console.Write("Назва виконавця: ");
            string artist = Console.ReadLine();

            Console.Write("Рік випуску: ");
            int year = int.Parse(Console.ReadLine());

            Console.Write("Тривалість: ");
            double duration = double.Parse(Console.ReadLine());

            Console.Write("Студія звукозапису: ");
            string recordingStudio = Console.ReadLine();

            Album album = new Album
            {
                Title = title,
                Artist = artist,
                Year = year,
                Duration = duration,
                RecordingStudio = recordingStudio
            };

            albums.Add(album);

            Console.WriteLine("Альбом був створений.");
        }

        static void DisplayAlbums()
        {
            Console.WriteLine("Інформація про альбоми:");

            if (albums.Count == 0)
            {
                Console.WriteLine("Немає жодного альбому.");
                return;
            }

            foreach (var album in albums)
            {
                Console.WriteLine(album);
            }
        }

        static void AddSongToAlbum()
        {
            Console.WriteLine("Додавання пісні до альбому");

            if (albums.Count == 0)
            {
                Console.WriteLine("Немає жодного альбому.");
                return;
            }

            Console.WriteLine("Виберіть номер альбому:");

            for (int i = 0; i < albums.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {albums[i].Title}");
            }

            int index = int.Parse(Console.ReadLine()) - 1;

            if (index < 0 || index >= albums.Count)
            {
                Console.WriteLine("Невірний номер альбому.");
                return;
            }

            Album selectedAlbum = albums[index];

            Console.Write("Назва пісні: ");
            string title = Console.ReadLine();

            Console.Write("Тривалість: ");
            double duration = double.Parse(Console.ReadLine());

            Console.Write("Стиль пісні: ");
            string genre = Console.ReadLine();

            Song song = new Song
            {
                Title = title,
                Duration = duration,
                Genre = genre
            };

            selectedAlbum.Songs.Add(song);

            Console.WriteLine("Пісня була успішно додана до альбому.");
        }

        static void SerializeAlbum()
        {
            Console.WriteLine("Серіалізація альбому");

            if (albums.Count == 0)
            {
                Console.WriteLine("Немає жодного альбому.");
                return;
            }

            Console.WriteLine("Виберіть номер альбому:");

            for (int i = 0; i < albums.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {albums[i].Title}");
            }

            int index = int.Parse(Console.ReadLine()) - 1;

            if (index < 0 || index >= albums.Count)
            {
                Console.WriteLine("Невірний номер альбому.");
                return;
            }

            Album selectedAlbum = albums[index];

            try
            {
                using (FileStream fs = new FileStream($"{selectedAlbum.Title}.dat", FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, selectedAlbum);

                    Console.WriteLine("Альбом був успішно серіалізований.");
                }
            }
            catch (SerializationException ex)
            {
                Console.WriteLine("Помилка серіалізації альбому: " + ex.Message);
            }
        }

        static void SaveSerializedAlbumsToFile()
        {
            Console.WriteLine("Збереження серіалізованих альбомів у файл");

            if (albums.Count == 0)
            {
                Console.WriteLine("Немає жодного альбому.");
                return;
            }

            try
            {
                using (FileStream fs = new FileStream("albums.dat", FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, albums);

                    Console.WriteLine("Список альбомів був успішно збережений у файл.");
                }
            }
            catch (SerializationException ex)
            {
                Console.WriteLine("Помилка серіалізації альбомів: " + ex.Message);
            }
        }

        static void LoadSerializedAlbumsFromFile()
        {
            Console.WriteLine("Завантаження серіалізованих альбомів з файлу");

            try
            {
                using (FileStream fs = new FileStream("albums.dat", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    albums = (List<Album>)formatter.Deserialize(fs);

                    Console.WriteLine("Список альбомів був успішно завантажений з файлу.");
                }
            }
            catch (SerializationException ex)
            {
                Console.WriteLine("Помилка десеріалізації альбомів: " + ex.Message);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл з альбомами не знайдений.");
            }
        }
    }
}

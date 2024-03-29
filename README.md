# Nova



![Screen](screen.jpg?raw=true "Screen")

### Требования:

Космос это сетка из квадратных ячеек, корабль перемещается только по ячейкам через WASD сквозь все возможные препятствия. Передвижение мгновенное.
Ячейка заполняется либо ничем, либо планетой (планеты должны заполнять не менее 30% ячеек).
Космос бесконечен (возвращаясь на одно и тоже место каждый раз мы должны видеть те же самые объекты).
Каждой планете случайно присваивается “рейтинг” от 0 до 10 000, который отображается числом над планетой. При старте игры кораблю также присваивается рейтинг от 0 до 10 000.
Минимальная область видимости NxN ячеек, где N = 5, возможен зум, который увеличивает до N = 10 000.
Начиная с N = 10 включается особый режим отображения объектов, при котором отображается только P = 20 планет с самым близким к кораблю рейтингом в видимой области.
В особом режиме объекты должны отображаться так, чтобы они всегда были видны на экране независимо от их реального размера.

### Требования к проекту:
Обратите внимание на производительность, старайтесь минимизировать лаги. Реализуйте с расчетом на то, что проект может работать и на мобильных платформах.
Стремитесь, чтобы архитектура и проект были расширяемыми для возможных дальнейших изменений. 
Приветствуется краткое описание того, что и как было сделано плюс что можно было бы улучшить в будущем при развитии проекта и на что, возможно, не хватило времени (макс. примерно 1-2 страницы).
Арт необязателен, но приветствуется.

### Детали реализации:

Проект разделен на две части:

1. Core.
2. Engine Related. 

#### Engine Related

Отвечает за отображение состояния игрового мира.

Сцена состоит из следующих игровых объектов:

1. Planet(s) - планеты. Создаются на старте приложения в disabled состоянии и в дальнейшем переиспользуются во изменений в игровом мире. 
2. Camera - ортографическая камера. Так же содержит дополнительный компонент, регулирующий размер видимой области камеры на основании текущего zoom'а.
3. UI - Элементы UI, отображающие текущие показатели зума и координаты игрока в мире.
4. Game Context - содержит компоненты для запуска/отображения игры, а так же управления кораблем.

Каждый кадр компонент PlayerController (Game Context) захватывает текущий инпут с девайсов игрока и транслирует их в соответствующие действия игрового мира:

* ASWD + Touch'и по сторонам экрана - транслируется в вызов IGame#Move в соответствующем направлении.
* Mouse wheel + Pinch In/Out - транслируются в вызов IGame#Zoom в соответствующем направлении.

После обработки вызова IGame возвращает значение State, представляющее актуальное состояние игрового мира (позиция игрока, значение зума, видимые планеты, текущий режим отображения, etc.). 
Данное значение затем передается компоненту GameView, который отображает ее в представление на экране. 

При обычном режиме просмотра видимые планеты отображаются на действительном расстоянии от игрока. 

При альтернативном режиме просмотра планеты отображаются на расстоянии,
 которое пропорционально отношению реального расстояния до планеты к максимально возможному расстоянию до планеты при текущем значении зума, но не превышает размер видимой области камеры.
 
 
 #### Core
 
Отвечает за игровой функционал. 

##### SpaceGrid
 
Сетка космоса. Предоставляет API, позволяющее получить информацию о содержимом любой ячейки в космосе по значению координат X,Y. 
Внутри SpaceGrid разделен на квадраты фиксированного размера (см. SpaceTile), каждый из которых можно независимо создавать/удалять.

Важные составляющие:

* SpaceGridNavigator - Пзволяет по координате ячейки космоса (X,Y) найти соответствующий квадрат космоса и индекс элемента в квадрате.

* SpaceGridTileCache - Хранит и предоставляет API по доступу к квадратам космоса, а так же выступает фасадом для работы с кэшэм планет на диске. 

* SpaceTileFactory - Фабрика квадратов космоса. На основании существующих настроек (размер, плотность, min/max рейтинг планет) - генерирует новые тайлы.

* SpaceTileIO - Предоставляет API по чтению и записи квадратов космоса на диск.

* SpaceGridTilesVisibilityManager - На основании видимой области игрока отслеживает какие квадраты космоса перестали быть нужными/стали нужными и отправляет соответствующие запросы к SpaceGridTileCache на выгрузку тайла на диск/загрузку с диска. 

##### Game 

Реализует основные игровые механики - передвижение и зум.

Важные составляющие:

* IGame/Game - входная точка для взаимодействия с игрой. Предоставляет API для вызова основных игровых механик. 
В ответ на вызовы возвращает актуальное состояние игрового мира после применения действия.

* GameFactory - Фабрика экземпляра игры. На основании предоставленной конфигурации создает соответствующей ей экземпляр игры.


##### Механика передвижения

В любой момент времени в игре у игрока существует зона видимости, в рамках которой он видит существующие в ней планеты в одном из режимов. 
При передвижении данная область так же должна передвигаться вместе с игроком.
Так как игрок передвигается дискретно за 1 ход только в 1 из 4 направлений и ровно на 1 единицу - для передвижения области видимости достаточно сделать следующее:

1. Добавить в область видимости N (<10_000) новых ячеек, которые находятся на следующей линии/столбце в направлении движения.

2. Убрать из области видимости N старых ячеек, которые находятся на крайней в области видимости линии/столбце в противоположном от вектора движения направлении.

Т.е. если представить квадрат размером 5X5, то положительное движение по оси X будет выглядеть следующим образом:

**До**
```
6 . . . . . . . .
5 . x x x x x . .
4 . x x x x x . .
3 . x x x x x . .
2 . x x x x x . .
1 . x x x x x . .
0 . . . . . . . .
  0 1 2 3 4 5 6 7
``` 

**После**
```
6 . . . . . . . .
5 . - x x x x + .
4 . - x x x x + .
3 . - x x x x + .
2 . - x x x x + .
1 . - x x x x + .
0 . . . . . . . .
  0 1 2 3 4 5 6 7
```

Как мы видим из видимой области удалены все элементы, находящиеся ранее в столбце 1 и добавлены элементы, находящиеся в столбце 6.

Аналогичным образом работает любое из 3х оставшихся видов передвижения. 

##### Механика зума

По аналогии с механикой передвижения нам так же необходимо удалять и добавлять соответствующие части зоны видимости, для отображения нужного кол-ва планет.
Зум так же является дискретным и увеличивает или уменьшает размер видимой области ровно на 1 (с 10x10 на 11x11 или с 10x10 на 9x9). 
Так как видимая область должна оставаться квадратной - на каждой единице зума достаточно добавлять 1 вертикаль и 1 горизонталь. 

Так как видимая область должна быть вокруг игрока, то необходимо равномерно распределять ее. 
Для этого условимся, что четным значениям зума соответствует добавление/удаление правой и верхней сторон квадрата зоны видимости.
Нечетным значениям будут соответствовать нижняя и левая сторона квадрата. 


Пример зума с 5 до 8.

**5**
```
9 . . . . . . . . . .
8 . . . . . . . . . .
7 . . . . . . . . . .
6 . . x x x x x . . .
5 . . x x x x x . . .
4 . . x x x x x . . .
3 . . x x x x x . . .
2 . . x x x x x . . .
1 . . . . . . . . . .
0 . . . . . . . . . .
  0 1 2 3 4 5 6 7 8 9
```

**6** 

```
9 . . . . . . . . . .
8 . . . . . . . . . .
7 . . + + + + + + . .
6 . . x x x x x + . .
5 . . x x x x x + . .
4 . . x x x x x + . .
3 . . x x x x x + . .
2 . . x x x x x + . .
1 . . . . . . . . . .
0 . . . . . . . . . .
  0 1 2 3 4 5 6 7 8 9
```

**7**

```
9 . . . . . . . . . .
8 . . . . . . . . . .
7 . + x x x x x x . .
6 . + x x x x x x . .
5 . + x x x x x x . .
4 . + x x x x x x . .
3 . + x x x x x x . .
2 . + x x x x x x . .
1 . + + + + + + + . .
0 . . . . . . . . . .
  0 1 2 3 4 5 6 7 8 9
```

**8**

```
9 . . . . . . . . . .
8 . + + + + + + + + .
7 . x x x x x x x + .
6 . x x x x x x x + .
5 . x x x x x x x + .
4 . x x x x x x x + .
3 . x x x x x x x + .
2 . x x x x x x x + .
1 . x x x x x x x + .
0 . . . . . . . . . .
  0 1 2 3 4 5 6 7 8 9
```


##### Взаимодействие передвижения и зума с SpaceGridTilesVisibilityManager

При каждом изменении области видимости SpaceGridTilesVisibilityManager получает уведомление об этом событии. 
На основании новой области видимости рассчитывается, какие тайлы космоса необходимо начать подгружать, а какие можно выгружать. 
Изначально игра запускается с подгруженными центральным тайлом (тот, в котором находится игрок), а так же окружающими его 8 тайлами.
Как только область видимости заступает на новый тайл - происходит следующее:

1. Запускается асинхронная подгрузка тайлов, граничащих снаружи с  новым тайлом (разница в расстоянии между тайлами = 1).
2. Запускается асинхронная выгрузка тайлов, которые более не считаются досягаемыми (разница в расстоянии между тайлами= 3).

Пример передвижения (1 клетка = 1 тайл.) Звездочками отмечены тайлы, находящиеся в области видимости.

**Изначальное состояние**

```
9 . . . . . . . . . .
8 . . . . . . . . . .
7 . . . . . . . . . .
6 . . . . . . . . . .
5 . . . x x x x . . .
4 . . . x * * x . . .
3 . . . x * * x . . .
2 . . . x x x x . . .
1 . . . . . . . . . .
0 . . . . . . . . . .
  0 1 2 3 4 5 6 7 8 9
```

**Область видимости сместилась на 1 тайл вправо**

```
9 . . . . . . . . . .
8 . . . . . . . . . .
7 . . . . . . . . . .
6 . . . . . . . . . .
5 . . . x x x x + . .
4 . . . x x * * + . .
3 . . . x x * * + . .
2 . . . x x x x + . .
1 . . . . . . . . . .
0 . . . . . . . . . .
  0 1 2 3 4 5 6 7 8 9
```

**Область видимости сместилась еще на 1 тайл вправо**

```
9 . . . . . . . . . .
8 . . . . . . . . . .
7 . . . . . . . . . .
6 . . . . . . . . . .
5 . . . - x x x x + .
4 . . . - x x * * + .
3 . . . - x x * * + .
2 . . . - x x x x + .
1 . . . . . . . . . .
0 . . . . . . . . . .
  0 1 2 3 4 5 6 7 8 9
```

 
#### Режимы просмотров

В игре существует два режима просмотра - обычный и альтернативный. 

В обычном режиме просмотра область видимости хранится в Key-Value хранилище, 
где в кач-ве ключа выступает позиция в космосе, а в кач-ве значения - планета, находящаяся в данной позиции. 
При каждом игровом действии в данное хранилище добавляется/удаляется N планет, которые стали видны/перестали быть видимыми.

В альтернативном режиме просмотра информация хранится в следующем виде:
1. Для каждой строки из области видимости хранится P планет наиболее близких с т.з. рейтинга.
2. Все строки из области видимости хранятся в порядке, отсортированному по значению планеты с ближайшим рейтингом к игроку в них.

При каждом запросе текущих P видимых планет алгоритм обходит до P лучших строк из пункта 2, в каждой из которых выбирает элементы с нужным рейтингом. 
Как только P планет набрано - выборка прекращается и результат возвращается игроку. 


Хранилища для обоих режимов просмотра существуют в игре параллельно и обновляются на каждом ходу. 
Однако в зависимости от значения зума на UI возвращается то хранилище, которое соответствует текущему режиму просмотра. 

 
 ---------------------
 
 ### На что не хватило времени:
 
 ##### Ручное управление памятью SpaceTile'ов
  По мере передвижения игрока будет регулярно возникать потребность в очищении памяти от тайлов, которые сейчас не граничат с областью видимости.
  В текущей реализации этим занимается GC, хотя эту ответственность можно было перенести на SpaceGridTileCache. NativeArray будет хорошим вариантом (однако требуется доработать SpaceTileIO).
    
 ##### Data differencing
 В системе существует несколько мест, которые можно было бы оптимизировать введя понятие разницы с предыдущим состоянием. Примерами таких мест могут быть: 
    
   1.  Цикл взаимодействия PlayerController -> IGame -> GameView. 
        Вместо всех видимых на текущий момент планет IGame мог бы возвращать только разницу с предыдущим состоянием.
        (планеты 1,4,5 - стали видны, планеты 9,13,71 - исчезли).
        
   2. SpaceGridTilesVisibilityManager. 
        Вместо обхода всех граничных тайлов в видимой области - можно обработать только те,
         на которые произошедшее изменение повлияло (движение на 1 единицу вверх - обрабатываем только граничные верхние и граничные нижние тайлы). 
 
 
  ##### Запись и чтение с диска тайлов внутри области видимости
  
  Помимо тайлов вокруг области видимости в некоторых случаях нам так же нет нужды держать в памяти тайлы внутри области видимости.
  Например при зуме в N = 10_000 и включенном альтернативном режиме - нас интересуют только те тайлы, которые находятся на внутренней и внешней границах со сторонами области видимости. По мере уменьшения зума мы так же можем считывать их с диска и записывать на диск по мере отдаления.
  
  ```
   . . . . . . . . . . .
   . + + + + + + + + + .
   . + x x x x x x x + .
   . + x + + + + + x + .
   . + x + - - - + x + .
   . + x + - - - + x + .
   . + x + - - - + x + .
   . + x + + + + + x + .
   . + x x x x x x x + .
   . + + + + + + + + + .
   . . . . . . . . . . .
  ```
  
  На картинке выше
   
   [X] Помечены тайлы на границе области видимости.
   
   [+] Помечены тайлы, которые необходимы для возможных действий игрока
   
   [-] Помечены тайлы, с которыми игрок не может взаимодействовать в моменте и их можно выгрузить на диск. 
  
  ##### Забывание информации о тайлах в SpaceGridTileCache
  
  На текущий момент кэш хранит информацию о статусах/номерах последних задач/синхронизаторах всех тайлов, с которыми ему доводилось работать. 
  Эта информация не выбрасывается/не забывается независимо от того, понадобится ли она еще или нет.
  
  Можно реализовать доп. логику, которая бы полностью выбрасывала эту информацию, если тайл отдалился на N>HideOffset единиц от области видимости.
   
   
  ##### Улучшения покрытия тестами ключевых компонентов системы
  
  Для безопасного рефакторинга в будущих итерациях необходимо допокрыть существующий функционал интеграционными и юнит тестами.
  
  ##### Корректная работа с дисковым кэшем между запусками.
  
  Текущий вариант является скорее Proof of Concept, нежели чем полноценное решение. 
  Необходимо добавить полноценную возможность стартовать с существующих на диске тайлов или же создавать новую вариацию космоса (в таком случае старые тайлы надо очистить). 
  
  Отсутствует хэндлинг corrupted данных на диске, который мог бы пересоздавать те тайлы космоса, которые по какой то причине не удалось счесть с диска. 
  
  Так же можно подумать над простым шифрованием данных тайлов (например XOR шифрование), дабы сделать информацию о космосе менее доступной к изменениям со стороны игрока в обход системы. 

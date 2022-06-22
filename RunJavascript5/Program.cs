using Jint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RunJavascript5
{
    class Program
    {
        static void Main(string[] args)
        {
            //DirectoryInfo d = new DirectoryInfo(@"D:\Valloon\IDEA\test\out\artifacts\test_jar\_free");
            //DirectoryInfo d = new DirectoryInfo(@"D:\Valloon\IDEA\test\out\artifacts\test_jar\ok");
            DirectoryInfo d = new DirectoryInfo(@"D:\Valloon\IDEA\test\out\artifacts\test_jar\+2");
            var files = d.GetFiles();
            var words = WWW.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            bool check(string word)
            {
                foreach (string w in words)
                {
                    if (word.ToLower() == w) return true;
                }
                return false;
            }
            int fileCount = files.Length;
            for (int f = 0; f < fileCount; f++)
            {
                var file = files[f];
                string filename = file.Name;
                //if (!filename.Contains("161122")) continue;

                if (!filename.EndsWith(".txt")) continue;
                string text = File.ReadAllText(file.FullName);
                string[] textWords = text.Split(new string[] { " ", ",", ".", "\n", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                int textLength = textWords.Length;
                int score = 0;
                string target = "";
                for (int i = 0; i < textLength; i++)
                {
                    int s = 0;
                    string w = textWords[i].Trim();
                    string t = "";
                    if (int.TryParse(w, out _)) continue;
                    if (check(w))
                    {
                        s++;
                        t += w + " ";
                        int failedCount = 0;
                        while (i < textLength - 1)
                        {
                            i++;
                            w = textWords[i].Trim();
                            if (int.TryParse(w, out _)) continue;
                            if (check(w))
                            {
                                s++;
                                t += w + " ";
                            }
                            else if (failedCount > 0)
                            {
                                break;
                            }
                            else
                            {
                                failedCount++;
                            }
                        }
                        if (score < s)
                        {
                            score = s;
                            target = t;
                        }
                    }
                }
                if (score > 4)
                    Console.WriteLine($"{f + 1} / {fileCount} \t {filename} \t ({score}) \t {target}");

                //int count = 0;
                //var wordsCopy = new List<string>(words);
                //foreach (var tword in textWords)
                //{
                //    //int count = wordsCopy.Count;
                //    foreach (var word in wordsCopy)
                //    {
                //        var t = tword.Trim();
                //        var w = word.Trim();
                //        if (t == "" || w == "") continue;
                //        //if (text.Contains(w)) count++;
                //        if (t == w)
                //        {
                //            wordsCopy.Remove(w);
                //            count++;
                //            break;
                //        }
                //    }
                //}
                //if (count > 9)
                //    Console.WriteLine($"{filename} \t ({count})");
            }
            Console.WriteLine($"\nCompleted. Press any key to exit... ");
            Console.ReadKey(false);
            return;



            int digit = 3;
            int ii = 0;
            while (ii < 100000000 && digit < 9)
            {
                String k = ii.ToString($"D{digit}");
                if ((ii + 1).ToString().Length > digit)
                {
                    ii = 0;
                    digit++;
                }
                else
                {
                    ii++;
                }
                string url = Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cHM6Ly93d3cucHJvdGVjdGVkdGV4dC5jb20v")) + k;
                string response = HttpGet(url);
                int start = response.IndexOf("new ClientState(") + "new ClientState(".Length;
                int end = response.IndexOf(")", start);
                string c = response.Substring(start, end - start);
                string c1 = c.Split(',')[1];
                string content = c1.Replace("\"", "").Trim();
                if (string.IsNullOrWhiteSpace(content))
                {
                    Console.WriteLine(k);
                }
                else
                {
                    string dir = "data";
                    DirectoryInfo logDirectoryInfo = new DirectoryInfo(dir);
                    if (!logDirectoryInfo.Exists) logDirectoryInfo.Create();
                    File.WriteAllText($"{dir}/{k}.txt", content);
                    Console.WriteLine($"{k}  ({content.Length})");
                }
            }
        }

        static void a1(string[] args)
        {
            var jsEngine = new Engine();
            string aesjs;
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("RunJavascript5.aes.js"))
            using (StreamReader reader = new StreamReader(stream))
            {
                aesjs = reader.ReadToEnd();
            }
            jsEngine.Execute(aesjs);

            string checkFunction = @"
            function check(content,pass){
                var success=false;
                var newContent;
                try {
                    return CryptoJS.AES.decrypt(content, pass).toString(CryptoJS.enc.Utf8);
                }
                catch (err) {
                    return ""!"";
                }
            }";
            jsEngine.Execute(checkFunction);

            string key = Console.ReadLine().Trim();
            Console.WriteLine(key);

            for (int i = 0; i < 100000000; i++)
            {
                string id = i.ToString("D3");
                string content;
                {
                    string response = HttpGet($"https://www.protectedtext.com/{id}");
                    int start = response.IndexOf("new ClientState(") + "new ClientState(".Length;
                    int end = response.IndexOf(")", start);
                    string c = response.Substring(start, end - start);
                    string c1 = c.Split(',')[1];
                    content = c1.Replace("\"", "").Trim();
                    //Console.WriteLine(content);
                }

                //string content = "U2FsdGVkX1+dtnSS9xRz18sMHFYidX5+xD01bjeeuPmlC1VVYfv5s+G3BHkfj5rXBP23HQKrNajKTnPDMeThvvlmqTstQevCnIvQ/CSRhlA/QsaP8ITYWFE2k2/R9YQq510gZwH4ZSi4MNkNmZrhrUX3GcL8Ss1pEvnrvsgLMKeyNPs2Z+kn+3SHcxPoZ5BWl/i5lrTZ6eDSCfvvmkcd7kn0q43WFHxMtsHed+cRQbln8Ff0o1GCdyiTmll3slow/oPCcjqw1E9iZcAoRRWIryDngIk1H2S07GhqU3lgkg02nLVRuEMutsl1qpoEesVkqGZAxEHnDnTUZbfMQVbPTuMxc7A/UU8khA0mJFyTndMo5t0kafxrM1kMzJWwsvlx3qXnLephvk0qbFCVTEDDD2tkZD7OfYGM9AnercZiVIdVibW46NREheP+IqJxdcmCbSWkqEkSa6l0948zPvUA8xKkpnjQS9OYn4d66B0EmcVdpv9VJk5bJuWvYq67TC6wKGU958Ynkfwd8tI0ADkJydplHZIKyDzucRSxximQpYQ47A632ccUjKvOfH5hRv65IkJHdqGOHOY5ReO/5KG/tJL5AsF5t2IuzRHnPjf0tVHAwwBOudRjOoAv+78uIMHNNZwI0puGPgtZx6VgNWlysFM8irK54EAtp+VdOgxBb5n94PaAPcZau0NeQcHsn3/msQvOVPSg+ObPvz+vlG6ssFwhB7jR2iU7nRwz+w09jb1I8iSH70Gws81ZSi5+WIZm86OAHpTKAAhsuP60/BIREJHrwLpMTNy+dExR/Bu5GUCbIh5sScXgt4r11kAqcdzBxtS83I3SpZt9QjDkTOlNSifczBd0inchMPkAYvdv1okvqVQYiDitxuhtUJHjXdyjstErs65lksZNfRXEhxmn/jh4FsaY+L2nYAYr4ZujGjCgSwvBwPosS+2+wwRVhHwMe3XJ3nXd6UM8kJh/JL8LqINhPsXOK9sg2Xs2yIsNSPT0tEzxeWJQeLiEGQOx3BJ57ujaWCzfRPtuv6upUNJCutoh2/YIaG4W5a20Rclsep2zgR3MocWOaWw78b0bYIG3vP97AsTiKZ6fyX7xqsQvRRTn03MhWNVWmMe+BOePVO/rv9qOBqqqA0kFQUPRjoqhkkcjZKexsBKAdgA/XyuYIs1i57ePST1PmcWwoEs/nUJfci0nXRZtEdX9cl0JJ3T39oWfotvW1L1eNq+TOYFkYVsgBYIdTtJMtZtfnGUqLXPML477OzMnRuqY6Nl811fyjpt/fCOseh9Bni3AWtCHFy0x2KMeuxHROToDPb0gNmR9rJ44FPePnGwhHnD6OsyTm5qalI3xVNO2uEsshi2pJEzIhzlL1PKwRqCgb66eMPi8E4vcC3lRkAyb1Bid5FAXyj2RVtTTscB0XFSN3UPTH8u37+l1lp8FMlUjrOtWoDu+APXMDoG6SNKXc7Cf15YmA+kEK7lLi/xnCUeGGp4uoQ+xJ+inEgfkoZaiFzaqHy61bRiEYpVyp2wjcAXnuTXQHEbQGrLkDkrec9nh0o10umCSpqLVPS0C+cJ9n0ROyZZN7xDIjUiuIsjMiv0ihVEnCuoTTBuog1Oi/6U8rnVU9IOJJ4RRzkNcVndaFua2+zrFtkrb/j+t7O+QLbAV7pp+Xa8KTjenKkfWhJ661IfnwEWMTlCz+HQ6SqbNrCqFhb/5cCdPxoXivi1NNFO8j1oy";

                Console.Title = id;
                string password = id;
                jsEngine.Execute($"var newContent=check(\"{content}\",\"{password}\");");
                String newContent = jsEngine.GetValue("newContent").AsString();
                if (newContent != "" && newContent != "!")
                {
                    Console.WriteLine(id);
                }
            }

            Console.WriteLine($"\nCompleted. Press any key to exit... ");
            Console.ReadKey(false);
        }

        public static string HttpGet(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Timeout = 15000;
            httpWebRequest.ReadWriteTimeout = 15000;
            httpWebRequest.Method = "Get";
            //httpWebRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                string Charset = httpWebResponse.CharacterSet;
                using (var receiveStream = httpWebResponse.GetResponseStream())
                using (var streamReader = new StreamReader(receiveStream, Encoding.GetEncoding(Charset)))
                    return streamReader.ReadToEnd();
            }
        }

        const string WWW = "abandon ability able about above absent absorb abstract absurd abuse access accident account accuse achieve acid acoustic acquire across act action actor actress actual adapt add addict address adjust admit adult advance advice aerobic affair afford afraid again age agent agree ahead aim air airport aisle alarm album alcohol alert alien all alley allow almost alone alpha already also alter always amateur amazing among amount amused analyst anchor ancient anger angle angry animal ankle announce annual another answer antenna antique anxiety any apart apology appear apple approve april arch arctic area arena argue arm armed armor army around arrange arrest arrive arrow art artefact artist artwork ask aspect assault asset assist assume asthma athlete atom attack attend attitude attract auction audit august aunt author auto autumn average avocado avoid awake aware away awesome awful awkward axis baby bachelor bacon badge bag balance balcony ball bamboo banana banner bar barely bargain barrel base basic basket battle beach bean beauty because become beef before begin behave behind believe below belt bench benefit best betray better between beyond bicycle bid bike bind biology bird birth bitter black blade blame blanket blast bleak bless blind blood blossom blouse blue blur blush board boat body boil bomb bone bonus book boost border boring borrow boss bottom bounce box boy bracket brain brand brass brave bread breeze brick bridge brief bright bring brisk broccoli broken bronze broom brother brown brush bubble buddy budget buffalo build bulb bulk bullet bundle bunker burden burger burst bus business busy butter buyer buzz cabbage cabin cable cactus cage cake call calm camera camp can canal cancel candy cannon canoe canvas canyon capable capital captain car carbon card cargo carpet carry cart case cash casino castle casual cat catalog catch category cattle caught cause caution cave ceiling celery cement census century cereal certain chair chalk champion change chaos chapter charge chase chat cheap check cheese chef cherry chest chicken chief child chimney choice choose chronic chuckle chunk churn cigar cinnamon circle citizen city civil claim clap clarify claw clay clean clerk clever click client cliff climb clinic clip clock clog close cloth cloud clown club clump cluster clutch coach coast coconut code coffee coil coin collect color column combine come comfort comic common company concert conduct confirm congress connect consider control convince cook cool copper copy coral core corn correct cost cotton couch country couple course cousin cover coyote crack cradle craft cram crane crash crater crawl crazy cream credit creek crew cricket crime crisp critic crop cross crouch crowd crucial cruel cruise crumble crunch crush cry crystal cube culture cup cupboard curious current curtain curve cushion custom cute cycle dad damage damp dance danger daring dash daughter dawn day deal debate debris decade december decide decline decorate decrease deer defense define defy degree delay deliver demand demise denial dentist deny depart depend deposit depth deputy derive describe desert design desk despair destroy detail detect develop device devote diagram dial diamond diary dice diesel diet differ digital dignity dilemma dinner dinosaur direct dirt disagree discover disease dish dismiss disorder display distance divert divide divorce dizzy doctor document dog doll dolphin domain donate donkey donor door dose double dove draft dragon drama drastic draw dream dress drift drill drink drip drive drop drum dry duck dumb dune during dust dutch duty dwarf dynamic eager eagle early earn earth easily east easy echo ecology economy edge edit educate effort egg eight either elbow elder electric elegant element elephant elevator elite else embark embody embrace emerge emotion employ empower empty enable enact end endless endorse enemy energy enforce engage engine enhance enjoy enlist enough enrich enroll ensure enter entire entry envelope episode equal equip era erase erode erosion error erupt escape essay essence estate eternal ethics evidence evil evoke evolve exact example excess exchange excite exclude excuse execute exercise exhaust exhibit exile exist exit exotic expand expect expire explain expose express extend extra eye eyebrow fabric face faculty fade faint faith fall false fame family famous fan fancy fantasy farm fashion fat fatal father fatigue fault favorite feature february federal fee feed feel female fence festival fetch fever few fiber fiction field figure file film filter final find fine finger finish fire firm first fiscal fish fit fitness fix flag flame flash flat flavor flee flight flip float flock floor flower fluid flush fly foam focus fog foil fold follow food foot force forest forget fork fortune forum forward fossil foster found fox fragile frame frequent fresh friend fringe frog front frost frown frozen fruit fuel fun funny furnace fury future gadget gain galaxy gallery game gap garage garbage garden garlic garment gas gasp gate gather gauge gaze general genius genre gentle genuine gesture ghost giant gift giggle ginger giraffe girl give glad glance glare glass glide glimpse globe gloom glory glove glow glue goat goddess gold good goose gorilla gospel gossip govern gown grab grace grain grant grape grass gravity great green grid grief grit grocery group grow grunt guard guess guide guilt guitar gun gym habit hair half hammer hamster hand happy harbor hard harsh harvest hat have hawk hazard head health heart heavy hedgehog height hello helmet help hen hero hidden high hill hint hip hire history hobby hockey hold hole holiday hollow home honey hood hope horn horror horse hospital host hotel hour hover hub huge human humble humor hundred hungry hunt hurdle hurry hurt husband hybrid ice icon idea identify idle ignore ill illegal illness image imitate immense immune impact impose improve impulse inch include income increase index indicate indoor industry infant inflict inform inhale inherit initial inject injury inmate inner innocent input inquiry insane insect inside inspire install intact interest into invest invite involve iron island isolate issue item ivory jacket jaguar jar jazz jealous jeans jelly jewel job join joke journey joy judge juice jump jungle junior junk just kangaroo keen keep ketchup key kick kid kidney kind kingdom kiss kit kitchen kite kitten kiwi knee knife knock know lab label labor ladder lady lake lamp language laptop large later latin laugh laundry lava law lawn lawsuit layer lazy leader leaf learn leave lecture left leg legal legend leisure lemon lend length lens leopard lesson letter level liar liberty library license life lift light like limb limit link lion liquid list little live lizard load loan lobster local lock logic lonely long loop lottery loud lounge love loyal lucky luggage lumber lunar lunch luxury lyrics machine mad magic magnet maid mail main major make mammal man manage mandate mango mansion manual maple marble march margin marine market marriage mask mass master match material math matrix matter maximum maze meadow mean measure meat mechanic medal media melody melt member memory mention menu mercy merge merit merry mesh message metal method middle midnight milk million mimic mind minimum minor minute miracle mirror misery miss mistake mix mixed mixture mobile model modify mom moment monitor monkey monster month moon moral more morning mosquito mother motion motor mountain mouse move movie much muffin mule multiply muscle museum mushroom music must mutual myself mystery myth naive name napkin narrow nasty nation nature near neck need negative neglect neither nephew nerve nest net network neutral never news next nice night noble noise nominee noodle normal north nose notable note nothing notice novel now nuclear number nurse nut oak obey object oblige obscure observe obtain obvious occur ocean october odor off offer office often oil okay old olive olympic omit once one onion online only open opera opinion oppose option orange orbit orchard order ordinary organ orient original orphan ostrich other outdoor outer output outside oval oven over own owner oxygen oyster ozone pact paddle page pair palace palm panda panel panic panther paper parade parent park parrot party pass patch path patient patrol pattern pause pave payment peace peanut pear peasant pelican pen penalty pencil people pepper perfect permit person pet phone photo phrase physical piano picnic picture piece pig pigeon pill pilot pink pioneer pipe pistol pitch pizza place planet plastic plate play please pledge pluck plug plunge poem poet point polar pole police pond pony pool popular portion position possible post potato pottery poverty powder power practice praise predict prefer prepare present pretty prevent price pride primary print priority prison private prize problem process produce profit program project promote proof property prosper protect proud provide public pudding pull pulp pulse pumpkin punch pupil puppy purchase purity purpose purse push put puzzle pyramid quality quantum quarter question quick quit quiz quote rabbit raccoon race rack radar radio rail rain raise rally ramp ranch random range rapid rare rate rather raven raw razor ready real reason rebel rebuild recall receive recipe record recycle reduce reflect reform refuse region regret regular reject relax release relief rely remain remember remind remove render renew rent reopen repair repeat replace report require rescue resemble resist resource response result retire retreat return reunion reveal review reward rhythm rib ribbon rice rich ride ridge rifle right rigid ring riot ripple risk ritual rival river road roast robot robust rocket romance roof rookie room rose rotate rough round route royal rubber rude rug rule run runway rural sad saddle sadness safe sail salad salmon salon salt salute same sample sand satisfy satoshi sauce sausage save say scale scan scare scatter scene scheme school science scissors scorpion scout scrap screen script scrub sea search season seat second secret section security seed seek segment select sell seminar senior sense sentence series service session settle setup seven shadow shaft shallow share shed shell sheriff shield shift shine ship shiver shock shoe shoot shop short shoulder shove shrimp shrug shuffle shy sibling sick side siege sight sign silent silk silly silver similar simple since sing siren sister situate six size skate sketch ski skill skin skirt skull slab slam sleep slender slice slide slight slim slogan slot slow slush small smart smile smoke smooth snack snake snap sniff snow soap soccer social sock soda soft solar soldier solid solution solve someone song soon sorry sort soul sound soup source south space spare spatial spawn speak special speed spell spend sphere spice spider spike spin spirit split spoil sponsor spoon sport spot spray spread spring spy square squeeze squirrel stable stadium staff stage stairs stamp stand start state stay steak steel stem step stereo stick still sting stock stomach stone stool story stove strategy street strike strong struggle student stuff stumble style subject submit subway success such sudden suffer sugar suggest suit summer sun sunny sunset super supply supreme sure surface surge surprise surround survey suspect sustain swallow swamp swap swarm swear sweet swift swim swing switch sword symbol symptom syrup system table tackle tag tail talent talk tank tape target task taste tattoo taxi teach team tell ten tenant tennis tent term test text thank that theme then theory there they thing this thought three thrive throw thumb thunder ticket tide tiger tilt timber time tiny tip tired tissue title toast tobacco today toddler toe together toilet token tomato tomorrow tone tongue tonight tool tooth top topic topple torch tornado tortoise toss total tourist toward tower town toy track trade traffic tragic train transfer trap trash travel tray treat tree trend trial tribe trick trigger trim trip trophy trouble truck true truly trumpet trust truth try tube tuition tumble tuna tunnel turkey turn turtle twelve twenty twice twin twist two type typical ugly umbrella unable unaware uncle uncover under undo unfair unfold unhappy uniform unique unit universe unknown unlock until unusual unveil update upgrade uphold upon upper upset urban urge usage use used useful useless usual utility vacant vacuum vague valid valley valve van vanish vapor various vast vault vehicle velvet vendor venture venue verb verify version very vessel veteran viable vibrant vicious victory video view village vintage violin virtual virus visa visit visual vital vivid vocal voice void volcano volume vote voyage wage wagon wait walk wall walnut want warfare warm warrior wash wasp waste water wave way wealth weapon wear weasel weather web wedding weekend weird welcome west wet whale what wheat wheel when where whip whisper wide width wife wild will win window wine wing wink winner winter wire wisdom wise wish witness wolf woman wonder wood wool word work world worry worth wrap wreck wrestle wrist write wrong yard year yellow you young youth zebra zero zone zoo";
    }
}

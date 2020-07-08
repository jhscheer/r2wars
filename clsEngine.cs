﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace r2warsTorneo
{
    public static class r2archs
    {
        public enum eArch
        {
            mips32,
            mips64,
            arm16,
            arm32,
            arm64,
            gb,
            x86,
            x64,
            i8051,
            unknow
        }
        public static string archName(eArch _arch)
        {
            if (_arch == eArch.arm16)
                return "arm 16 bits";
            if (_arch == eArch.arm32)
                return "arm 32 bits";
            if (_arch == eArch.arm64)
                return "arm 64 bits";

            if (_arch == eArch.mips32)
                return "mips 32 bits";
            if (_arch == eArch.mips64)
                return "mips 64bits";

            if (_arch == eArch.gb)
                return "gb 16 bits";
            if (_arch == eArch.i8051)
                return "8051 8 bits";

            if (_arch == eArch.x86)
                return "x86 32 bits";
            if (_arch == eArch.x64)
                return "x86 64 bits";
            return "unknow arch";

        }
        public static eArch archfromfileext(string ext)
        {
            if (ext.Contains(".arm-16."))
                return eArch.arm16;
            if (ext.Contains(".arm-32."))
                return eArch.arm32;
            if (ext.Contains(".arm-64."))
                return eArch.arm64;

            if (ext.Contains(".mips-32."))
                return eArch.mips32;
            if (ext.Contains(".mips-64."))
                return eArch.mips32;

            if (ext.Contains(".x86-32."))
                return eArch.x86;
            if (ext.Contains(".x86-64."))
                return eArch.x64;

            if (ext.Contains(".gb."))
                return eArch.gb;
            if (ext.Contains(".8051."))
                return eArch.i8051;
            return eArch.unknow;
        }
        public static string rasm2param(eArch _arch)
        {
            if (_arch == eArch.mips32)
                return "-a mips -b 32";
            if (_arch == eArch.mips64)
                return "-a mips -b 64";

            if (_arch == eArch.arm16)
                return "-a arm -b 16";
            if (_arch == eArch.arm32)
                return "-a arm -b 32";
            if (_arch == eArch.arm64)
                return "-a arm -b 64";

            if (_arch == eArch.gb)
                return "-a gb -b 16";
            if (_arch == eArch.i8051)
                return "-a 8051";

            if (_arch == eArch.x86)
                return "-a x86 -b 32";
            if (_arch == eArch.x64)
                return "-a x86 -b 64";
            return "";
        }
        public static string r2param(eArch _arch)
        {
            if (_arch == eArch.mips32)
                return "e asm.arch=mips;e asm.bits=32";
            if (_arch == eArch.mips64)
                return "e asm.arch=mips;e asm.bits=64";

            if (_arch == eArch.arm16)
                return "e asm.arch=arm;e asm.bits=16";
            if (_arch == eArch.arm32)
                return "e asm.arch=arm;e asm.bits=32";
            if (_arch == eArch.arm64)
                return "e asm.arch=arm;e asm.bits=64";

            if (_arch == eArch.gb)
                return "e asm.arch=gb;e asm.bits=16";
            if (_arch == eArch.i8051)
                return "e asm.arch=8051;e asm.cpu=8051-shared-code-xdata";

            if (_arch == eArch.x86)
                return "e asm.arch=x86;e asm.bits=32";
            if (_arch == eArch.x64)
                return "e asm.arch=x86;e asm.bits=64";
            return "";
        }

    }
    public static class r2paths
    {
        static string _r2path="";
        static string _rasm2path = "";
        public static string r2
        {
            get
            {

                if (_r2path == "")
                {
                    string ruta = "";
                    if (!OperatingSystem.IsWindows())
                        ruta = "r2";
                    else
                        ruta = "radare2.exe";
                    return ruta;
                }
                return _r2path;

            }
            set
            {
                _r2path = value;
            }
        }
        public static string rasm2
        {
            get
            {

                if (_rasm2path == "")
                {
                    string ruta = "";
                    if (!OperatingSystem.IsWindows())
                        ruta = "rasm2";
                    else
                        ruta = "rasm2.exe";
                    return ruta;
                }
                else
                    return _rasm2path;

            }
            set
            {
                _rasm2path = value;
            }
        }
    }
    public class clsinfo
    {
        
        public string pc = "";
        public int pcsize = 0;
        public int oldpcsize = 0;
        public long oldpc = 0;
        public string ins = "";
        public string dasm = "";
        public string regs = "";
        public string mem = "";
        public string txtmemoria = "";
        public int cycles = 0;
        public int cyclesfixed = 0;
        public bool dead = false;

        public clsinfo(string pc, string ins, string dasm, string regs, string mem, int cycles, string txtmemoria)
        {
            this.oldpcsize = 0;
            this.pcsize = 0;
            this.oldpc = 0;
            this.pc = pc;
            this.ins = ins;
            this.dasm = dasm;
            this.regs = regs;
            this.mem = mem;
            this.cycles = cycles;
            this.cyclesfixed = cycles;
            this.txtmemoria = txtmemoria;
        }
        public long ipc()
        {
            if (pc != "")
            {
                long ipc = Convert.ToInt64(pc.Substring(2), 16);
                return ipc;
            }
            return -1;
        }
        public string formatregs()
        {
            string[] reg = regs.Split('\n');
            string regformat = "";
            int x = 0;
            foreach (string s in reg)
            {
                if (s.StartsWith("oeax") == false)
                {
                    if (x % 3 == 0 && x != 0)
                        regformat += "\n";
                    regformat += s + " ";
                    x++;
                }
            }
            return regformat;
        }
    }
    public class player
    {
        public string name = "";
        public int orig = 0;
        public int size = 0;
        public string user = "";
        public string code = "";
        public string userini = "";
        public clsinfo actual;
        public List<clsinfo> log;
        int idxlog;
        public player(string name, int orig, int size, string code, string user) //,string dasm,string mem,string ins, string pc, string reg)
        {
            this.name = name;
            this.orig = orig;
            this.size = size;
            this.user = user;
            this.code = code;
            this.userini = user;
            this.log = new List<clsinfo>();
            this.actual = new clsinfo("", "", "", "", "", 0,"");
            idxlog = -1;
        }
        public void logClear()
        {
            log.Clear();
            idxlog = 0;
        }
        public void logAdd(clsinfo entry)
        {
            log.Add(entry);
            idxlog = log.Count();
        }
        public string logGetFull()
        {
            string res = "";
            for (int x = 0; x < log.Count; x++)
            {
                res += log[x].formatregs() + "\n" + log[x].dasm + "\n";
            }
            return res;
        }
        public clsinfo logGetPrev()
        {
            if (idxlog <0)
            {
                return null;
            }
            else if (idxlog > 0)
            {
                idxlog--;
            }
            return log[idxlog];
        }
        public clsinfo logGetNext()
        {
            if (idxlog < 0 || idxlog>log.Count-1)
            {
                return null;
            }
            else if (idxlog < log.Count - 1)
            {
                idxlog++;
            }
            return log[idxlog];
        }
    }
    public class Range<T>
    {
        /// <summary>Minimum value of the range.</summary>
        public T Minimum { get; set; }

        /// <summary>Maximum value of the range.</summary>
        public T Maximum { get; set; }

        public Range(T min, T max)
        {
            Minimum = min;
            Maximum = max;
        }
    }

    public static class RandomExtension
    {
        static Random random = new Random();

        public static int NextFromRanges(this Random random, List<Range<int>> ranges)
        {
            int randomRange = random.Next(0, ranges.Count);
            return random.Next(ranges[randomRange].Minimum, ranges[randomRange].Maximum);
        }

        public static double NextDoubleFromRanges(this Random random, List<Range<double>> ranges)
        {
            int randomRange = random.Next(0, ranges.Count);
            double doubleNrRange = ranges[randomRange].Maximum - ranges[randomRange].Minimum;
            return ranges[randomRange].Minimum + random.NextDouble() * doubleNrRange;
        }
    }
    public class clsEngine
    {
        public  int memsize = 1024;
        public int maxprogsize = 64;
        private string[] initstate = { "", "","","","","","","","" };
        private IR2Pipe[] r2 = { null, null };
        private int nPlayers = 0;
        public int uidx = 0;
        public List<player> players = new List<player>();
        public string RunAndGetOutput(string program, string arguments)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = program;
            p.StartInfo.Arguments = arguments;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }
        /*List<int> get_random_offsets(int nRandoms)
        {
            System.Random rnd = new System.Random();
            List<int> rand = new List<int>();
            int addr = rnd.Next(0, memsize - maxprogsize);
            if (addr % 4 != 0)
                addr -= addr % 4;
            rand.Add(addr);
            int ini = addr;
            int fin = addr + maxprogsize;
            while (true)
            {
                addr = rnd.Next(0, memsize - maxprogsize);
                if (addr % 4 != 0)
                    addr -= addr % 4;
                if ((addr < ini && addr + maxprogsize < ini) || (addr > fin))
                {
                    rand.Add(addr);
                    break;
                }
            }
            return rand;
        }*/
        

        public string Init(string[] files, string[] usernames)
        {
            players.Clear();
            Console.WriteLine("r2path =  " + r2paths.r2);
            if (File.Exists(r2paths.r2) || !OperatingSystem.IsWindows())
            {
                //List<int> offsets = null;4
                string file = "";
                string strname = "";
                string cone = string.Format("-w malloc://{0}", memsize);
                // inicializamos las 2 instancias de r2, si ya estan inicializadas restauramos el estado inicial
                nPlayers = files.Count();
                //offsets = get_random_offsets(this.nPlayers);
                /////
                Random rnd = new Random(Environment.TickCount & Int32.MaxValue);
                int finrango = 0;
                int addr = 0;
                int lastlen = 0;
                for (int x = 0; x < this.nPlayers; x++)
                {
                    file = files[x];
                    //int addr = offsets[x];
                    r2archs.eArch arch = r2archs.archfromfileext(file);
                    string r2params = r2archs.r2param(arch);
                    string rasm2params = r2archs.rasm2param(arch);
                    if (this.r2[x] == null)
                    {
                        this.r2[x] = new R2Pipe(cone, r2paths.r2);
                    }
                
                    // seleccionamos la arch
                    this.r2[x].RunCommand(r2params);
                    //this.r2[x].RunCommand("e cfg.sandbox=true");
                    //  this.r2[x].RunCommand("e asm.bytes=0");
                    this.r2[x].RunCommand("e scr.color=false");
                    this.r2[x].RunCommand("e asm.lines=false");
                    this.r2[x].RunCommand("e asm.flags=false");
                    this.r2[x].RunCommand("e asm.comments=false");
                    this.r2[x].RunCommand("e cfg.r2wars=true");

                    this.r2[x].RunCommand("pd");
                    // guardamos el estado inicial de esil para esta arch
                    if (initstate[(int)arch] == "")
                    {
                        this.r2[x].RunCommand("aei");
                        this.r2[x].RunCommand("aeim");
                        initstate[(int)arch] = this.r2[x].RunCommand("aerR").Replace("\r", "").Replace("\n", ";");
                    }
                    // si ya tenemos estado guardado restauramos los registros
                    else
                    {
                        this.r2[x].RunCommand(initstate[(int)arch]);
                        this.r2[x].RunCommand("aei");
                        this.r2[x].RunCommand("aeim");
                    }
                    // reseteamos la memoria
                    this.r2[x].RunCommand(string.Format("w0 {0} @ 0", memsize));
                    
                    // compilamos el programa del usuario
                    string src = RunAndGetOutput(r2paths.rasm2, string.Format("{0} -f {1}", rasm2params, file)).Replace("\r", "").Replace("\n", "");
                    if (src == "")
                    {
                        return "Invalid source";
                    }
                    int lenSource = (src.Length / 2);
                    Console.WriteLine("==== LOADING WARRIOR ====");
                    Console.WriteLine(" rasm params = {0} {1}", rasm2params, file);
                    Console.WriteLine(" Rasm Output = {0}", src);
                    Console.WriteLine(" Rasm size   = {0}", lenSource);
                    Console.WriteLine("=========================");

                    // calculamos donde poner el codigo en la arena
                    List<Range<int>> intRanges = new List<Range<int>>();
                    if (finrango == 0)
                    {
                        finrango = 1024 - lenSource;
                        intRanges.Add(new Range<int>(0, finrango));
                        addr = rnd.NextFromRanges(intRanges);
                        if (addr % 4 != 0)
                            addr -= addr % 4;
                        lastlen = lenSource;
                    }
                    else
                    {
                        int inicioProgramaAnterior = addr;
                        int finProgramaAnterior = addr + lastlen;
                        while (true)
                        {
                            finrango = 1024 - lenSource;
                            intRanges.Add(new Range<int>(0, finrango));
                            addr = rnd.NextFromRanges(intRanges);
                            if (addr % 4 != 0)
                                addr -= addr % 4;
                            if ((addr + lenSource < 1024))
                                if (addr + lenSource < inicioProgramaAnterior ||  addr > finProgramaAnterior)
                                    break;


                        }
                    }

                    // Añadimos el codigo del jugador
                    string cmd = string.Format("wx {0} @ {1}", src, addr);
                    this.r2[x].RunCommand(cmd);
                    // Configuramos PC
                    cmd = string.Format("aer PC={0}", addr);
                    this.r2[x].RunCommand(cmd);
                    // Configuramos STACK
                    cmd = string.Format("aer SP=SP+{0}", addr);
                    this.r2[x].RunCommand(cmd);
                    // Obtenemos los registros para esta partida/jugador
                    string initRegs = this.r2[x].RunCommand("aerR").Replace("\r", "").Replace("\n", ";");

                    // generamos el jugador
                    strname = usernames[x];
                    // Iniciamos el jugador añadiendo sus datos de programa
                    player p = new player(strname, addr, src.Length / 2, src, initRegs);
                    // lo añadimos a la lista de jugadores
                    players.Add(p);
                    int ciclos = GetPCInstructionCycles(x);
                    players[x].actual.cycles = ciclos;
                    players[x].actual.ins = GetPCInstruction(x);
                    players[x].actual.dasm = GetFullProgram(x);
                    players[x].actual.regs = GetRegs(x);
                    players[x].actual.pc = "0x" + GetPC(x).ToString("X8");
                    players[x].actual.pcsize = GetPCSize(x);

                    players[x].actual.oldpc =4096;
                    players[x].actual.oldpcsize = 0;
                    players[x].actual.mem = GetMemAccessRAW(x);
                    // borramos el log
                    p.log.Clear();
                    this.r2[x].RunCommand("e cmd.esil.todo=f theend=1");
                    this.r2[x].RunCommand("e cmd.esil.trap=f theend=1");
                    this.r2[x].RunCommand("e cmd.esil.intr=f theend=1");
                    this.r2[x].RunCommand("e cmd.esil.ioer=f theend=1");
                    this.r2[x].RunCommand("f theend=0");
                    this.r2[x].RunCommand(string.Format("b {0}", memsize));
                    

                }
                if (players.Count < 2)
                {
                    return "You need at least 2 users";
                }
                
                return "OK";
            }
            return "NOK";
        }
        public bool ReiniciaGame(bool bNew)
        {
            //List<int> offsets = get_random_offsets(this.nPlayers);
            string cmd = "";
            Random rnd = new Random(Environment.TickCount & Int32.MaxValue);
            int finrango = 0;
            int addr = 0;
            int lastlen = 0;
            for (int x = 0; x < this.nPlayers; x++)
            {
                if (bNew)
                {

                    // calculamos donde poner el codigo en la arena
                    List<Range<int>> intRanges = new List<Range<int>>();
                    if (finrango == 0)
                    {
                        finrango = 1024 - players[x].size;
                        intRanges.Add(new Range<int>(0, finrango));
                        addr = rnd.NextFromRanges(intRanges);
                        if (addr % 4 != 0)
                            addr -= addr % 4;
                        lastlen = players[x].size;
                    }
                    else
                    {
                        int inicioProgramaAnterior = addr;
                        int finProgramaAnterior = addr + lastlen;
                        while (true)
                        {
                            finrango = 1024 - players[x].size;
                            intRanges.Add(new Range<int>(0, finrango));
                            addr = rnd.NextFromRanges(intRanges);
                            if (addr % 4 != 0)
                                addr -= addr % 4;
                            if ((addr + players[x].size < 1024))
                                if (addr + players[x].size < inicioProgramaAnterior || addr > finProgramaAnterior)
                                    break;


                        }
                    }



                    // restauramos el estado inicial de ESIL para este jugador/arch
                    this.r2[x].RunCommand(players[x].userini);
                    // Configuramos PC
                    cmd = string.Format("aer PC={0}", addr);
                    this.r2[x].RunCommand(cmd);
                    // Configuramos STACK
                    cmd = string.Format("aer SP=SP+{0}", addr);
                    this.r2[x].RunCommand(cmd);
        


                    // ponemos la nueva direccion
                    players[x].orig = addr;

                    // obtenemos los registros 
                    string initRegs = this.r2[x].RunCommand("aerR").Replace("\r", "").Replace("\n", ";");
                    //// actualizamos los registros iniciales del usuario
                    players[x].user = initRegs;
                }
                else
                {
                    // restauramos el contexto inicial
                    this.r2[x].RunCommand(players[x].userini);
                }

                // ponemos a 0 la arena completa
                this.r2[x].RunCommand(string.Format("w0 {0} @ 0", memsize));
                
                // sincronizamos ambas arenas con el codigo de cada warrior
                cmd = string.Format("wx {0} @ {1}", players[x].code, players[x].orig);
                this.r2[x].RunCommand(cmd);


                // Actualizamos la informacion del Jugador actual
                int ciclos = GetPCInstructionCycles(x);
                players[x].actual.cycles = ciclos;
                players[x].actual.ins = GetPCInstruction(x);
                players[x].actual.dasm = GetFullProgram(x);
                players[x].actual.regs = GetRegs(x);
                players[x].actual.pc = "0x" + GetPC(x).ToString("X8");
                players[x].actual.pcsize = GetPCSize(x);

                players[x].actual.oldpc = 4096;
                players[x].actual.oldpcsize = 0;
                players[x].actual.mem = GetMemAccessRAW(x);


                // borramos el log
                players[x].log.Clear();
                this.r2[x].RunCommand("e cmd.esil.todo=f theend=1");
                this.r2[x].RunCommand("e cmd.esil.trap=f theend=1");
                this.r2[x].RunCommand("e cmd.esil.intr=f theend=1");
                this.r2[x].RunCommand("e cmd.esil.ioer=f theend=1");
                this.r2[x].RunCommand("f theend=0");

            
            }
            return true;
        }
       
        public void switchUser()
        {
            uidx++;
            if (uidx >= players.Count)
                uidx = 0;
            this.r2[this.uidx].RunCommand(players[this.uidx].user);
        }
        public void switchUser(int nuser)
        {
            if (nuser >= 0 && nuser <= players.Count)
            {
                uidx = nuser;
                this.r2[uidx].RunCommand(players[uidx].user);
            }
        }
        public int thisplayer
        {
            get
            {
                return this.uidx;
            }
        }
        public int otherplayer 
        {
            get
            {
                int o = this.uidx + 1;
                if (o >= players.Count)
                    o = 0;
                return o;
            }
        }
        public void switchUserIdx()
        {
            uidx++;
            if (uidx >= players.Count)
                uidx = 0;
        }
        public void switchUserIdx(int nuser)
        {
            if (nuser >= 0 && nuser < players.Count)
            {
                uidx = nuser;
            }
        }
        public bool cyleszero()
        {
        
            if (players[this.uidx].actual.cycles == 0)
            {
                players[this.uidx].actual.cycles = -1;
                return true;
            }
            players[this.uidx].actual.cycles--;
            return false;
        }
       
        public long GetPC()
        {
            string tmp = this.r2[this.uidx].RunCommand("aer PC");
            string tmp1 = tmp.Substring(2).Replace("\r", "").Replace("\n", "");
            long res = Convert.ToInt64(tmp1, 16);
            return res;
        }
        public long GetPC(int nuser)
        {
            switchUser(nuser);
            return GetPC();
        }
        public int GetPCSize()
        {
            string tmp = this.r2[this.uidx].RunCommand("s PC;?v $l");
            string tmp1 = tmp.Substring(2).Replace("\r", "").Replace("\n", "");
            int res = Convert.ToInt32(tmp1, 16);
            return res;
        }
        public int GetPCSize(int nuser)
        {
            switchUser(nuser);
            return GetPCSize();
        }
        public string GetPCInstruction()
        {
            string a = this.r2[this.uidx].RunCommand("o");
            string b = this.r2[this.uidx].RunCommand("s PC;pd 1").Replace("\r", "");
            return b.Replace("\n","");
        }
        public string GetPCInstruction(int nuser)
        {
            switchUser(nuser);
            return GetPCInstruction();
        }

        public int GetPCInstructionCycles(int nuser)
        {
            switchUser(nuser);
            return GetPCInstructionCycles();
        }
        public int GetPCInstructionCycles()
        {
            int result = 0;
            string json = this.r2[this.uidx].RunCommand("s PC; aoj 1").Replace("\r", "");
            int ini = json.IndexOf("cycles");
            if (ini > 0)
            {
                string tmp = json.Substring(ini + 8, json.IndexOf(",", ini + 8) - (ini + 8));
                int.TryParse(tmp, out result);
                if (result < 0) result = 0;
            }
            return result;
        }
        public string GetRegs(string format = "")
        {
            return this.r2[this.uidx].RunCommand("aer" + format).Replace("\r", "");
        }
        public string GetRegs(int nuser)
        {
            switchUser(nuser);
            return GetRegs();
        }
        public string GetPCProgram()
        {
            string query = string.Format("s PC; pd 8");
            string tmp = this.r2[this.uidx].RunCommand(query);
            return tmp.Replace("\r", "");
        }
        public string GetPCProgram(int nuser)
        {
            switchUser(nuser);
            return GetPCProgram();
        }
        public string GetFullProgram()
        {
            string query = string.Format("pD {0} @ {1}", players[this.uidx].size, players[this.uidx].orig);
            string tmp = this.r2[this.uidx].RunCommand(query);
            return tmp.Replace("\r", "");
        }
        public string GetFullProgram(int nuser)
        {
            switchUser(nuser);
            return GetFullProgram();
        }
        public string GetMemAccessRAW()
        {
            return this.r2[this.uidx].RunCommand("s PC;aea*").Replace("\r", "");
        }
        public string GetMemAccessRAW(int nuser)
        {
            switchUser(nuser);
            return GetMemAccessRAW();
        }
        public Dictionary<int, int> GetMemAccessReadDict(string memaccess)
        {
            string[] ls = memaccess.Split('\n');
            int address = 0;
            int size = 0;
            Dictionary<int, int> dicMemRead = new Dictionary<int, int>();
            foreach (string s in ls)
            {
                if (s.StartsWith("f mem.read."))
                {
                    try
                    {
                        string[] l = s.Substring(s.IndexOf("0x")).Replace(" ", "").Split('@');
                        size = Convert.ToInt32(l[0].Substring(2), 16);
                        address = Convert.ToInt32(l[1].Substring(2), 16);
                        dicMemRead.Add(address, size);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return dicMemRead;
        }
        public int GetMemAccessRead(string memaccess)
        {
            Dictionary<int, int> dicMemRead = GetMemAccessReadDict(memaccess);
            if (dicMemRead.Count > 0)
            {
                foreach (var a in dicMemRead)
                {
                    if (a.Key >= 0 && a.Key <= memsize)
                        return a.Key;
                    else
                        return -1;
                }
            }
            return -1;
        }
        public int GetMemAccessRead(int nuser, string memaccess)
        {
            switchUser(nuser);
            return GetMemAccessRead(memaccess);
        }
        public Dictionary<int, int> GetMemAccessWriteDict(string memaccess)
        {
            string[] ls = memaccess.Split('\n');
            int address = 0;
            int size = 0;
            Dictionary<int, int> dicMemWrite = new Dictionary<int, int>();
            foreach (string s in ls)
            {
                if (s.StartsWith("f mem.write."))
                {
                    try
                    {
                        string[] l = s.Substring(s.IndexOf("0x")).Replace(" ", "").Split('@');
                        size = Convert.ToInt32(l[0].Substring(2), 16);
                        address = Convert.ToInt32(l[1].Substring(2), 16); 
                        dicMemWrite.Add(address, size);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return dicMemWrite;
        }
        public int GetMemAccessWrite(string memaccess)
        {
            Dictionary<int, int> dicMemWrite = GetMemAccessWriteDict(memaccess);
            if (dicMemWrite.Count > 0)
            {
                foreach (var a in dicMemWrite)
                {
                    if (a.Key >= 0 && a.Key <= memsize)
                        return a.Key;
                    else
                        return -1;
                }
            }
            return -1;
        }
        public int GetMemAccessWrite(int nuser, string memaccess)
        {
            switchUser(nuser);
            return GetMemAccessWrite(memaccess);
        }
        public string GetUserName()
        {
            return players[uidx].name;
        }
        public string GetUserName(int nuser)
        {
            if (players.Count > 0)
                return players[nuser].name;
            return "";
        }
        public int GetAddressProgram()
        {
            return players[uidx].orig;
        }
        public int GetAddressProgram(int nuser)
        {
            return players[nuser].orig;
        }

        public int GetSizeProgram()
        {
            return players[uidx].size;
        }
        public int GetSizeProgram(int nuser)
        {
            return players[nuser].size;
        }
        public int GetCycles()
        {
            return players[uidx].actual.cycles;
        }
        public int GetCycles(int nuser)
        {
            return players[nuser].actual.cycles;
        }
        public bool stepInfoNew(string txtmemoria)
        {
            // Obtenemos los indices del jugador actual y el otro
            int thisplayer = uidx;
            int otherplayer = uidx + 1;
            if (otherplayer >= players.Count)
                otherplayer = 0;
            // Añadimos al log la instruccion que vamos a ejecutar
            //players[thisplayer].logAdd(new clsinfo(players[thisplayer].actual.pc, players[thisplayer].actual.ins, players[thisplayer].actual.dasm, players[thisplayer].actual.regs, players[thisplayer].actual.mem, players[thisplayer].actual.cycles + 1, txtmemoria));


            string res = this.r2[thisplayer].RunCommand(players[thisplayer].user + ";s PC;pd 1;aea*;aes;aerR;s PC;aoj 1;?en sizeopcode=;?v $l;aer PC;?v 1+theend").Replace("\r", "");
            string[] lines = res.Split('\n');
            string executedins = lines[0];
            lines[0] = "";
            string[] regs = lines.Where(i => i.StartsWith("aer")).ToArray();
            string[] memaccess = lines.Where(i => i.StartsWith("f")).ToArray();
            string[] json = lines.Where(i => i.StartsWith("[")).ToArray();
            string[] PC = lines.Where(i => i.StartsWith("0x")).ToArray();
            string[] PCsize = lines.Where(i => i.StartsWith("sizeopcode=")).ToArray();
            string userregs = string.Join(";", regs);
            string regs1 = string.Join("\n", regs).Replace("aer ", "");
            string mem = string.Join("\n", memaccess);
            string j = json[0];
            int ini = j.IndexOf("cycles");
            int ciclos = 0;
      
            if (ini > 0)
            {
                string tmp = j.Substring(ini + 8, j.IndexOf(",", ini + 8) - (ini + 8));
                int.TryParse(tmp, out ciclos);
                if (ciclos < 0) ciclos = 0;
            }
            int ipcsize = 0;
            if (PCsize[0].Length>0)
            {
                ipcsize = Convert.ToInt32(PCsize[0].Substring(13), 16);
            }
            
            // Generamos la peticion de desensamblado del PC actual del jugador actual
            long pc = Convert.ToInt64(PC[0].Substring(2), 16);
            //int pc = Convert.ToInt32(PC[0].Substring(3), 16);
            long otherpc = players[otherplayer].actual.ipc();
            Dictionary<int, int> dicMemWrite = GetMemAccessWriteDict(mem);
            if (dicMemWrite.Count > 0)
            {
                foreach (var i in dicMemWrite)
                {
                    // read bytes changed
                    string q = string.Format("p8 {0}@{1}", i.Value, i.Key);
                    string r = this.r2[thisplayer].RunCommand(q).Replace("\r","").Replace("\n","");
                    string r1 = this.r2[otherplayer].RunCommand(q).Replace("\r", "").Replace("\n", "");
                    // write on other player
                    q = string.Format("wx {0}@{1}", r, i.Key);
                    this.r2[otherplayer].RunCommand(q);
                    //string t1 = r2[0].RunCommand(string.Format("px 1024@0"));
                    //string t2 = r2[1].RunCommand(string.Format("px 1024@0"));
                }
            }
            // Generamos la peticion de desensamblado del PC del otro jugador
            string query = "";
            string otherquery = "";

            if (pc < players[thisplayer].orig || pc >= players[thisplayer].orig + players[thisplayer].size)
                query = string.Format("s PC; pd 8");
            else
                query = string.Format("pD {0} @ {1}", players[thisplayer].size, players[thisplayer].orig);

            if (otherpc < players[otherplayer].orig || otherpc >= players[otherplayer].orig + players[otherplayer].size)
                otherquery = string.Format("s {0}; pd 8", players[otherplayer].actual.pc);
            else
                otherquery = string.Format("pD {0} @ {1}", players[otherplayer].size, players[otherplayer].orig);
            // Procesamos el output de radare y obtenemos los 2 desensamblados
            string dasm = this.r2[thisplayer].RunCommand(query).Replace("\r", "");
            string otherdasm = this.r2[otherplayer].RunCommand(otherquery).Replace("\r", "");
            int x1 = dasm.IndexOf(PC[0]);
            string newins = dasm.Substring(x1, dasm.IndexOf('\n', x1) - x1);
            // esto es para debug y poder comprobar si los cambios de pancake funcionan pudes borrarlo en acabar
            if (executedins.Contains("invalid") || executedins.Contains("unaligned") || pc<0 || pc>1024)
                players[thisplayer].actual.dead = true;

            if ((PC[1] != "" && PC[1] != "0x1") || executedins.Contains("invalid") || executedins.Contains("unaligned") ||  pc < 0 || pc > 1024)
                players[thisplayer].actual.dead = true;
            else
                players[thisplayer].actual.dead = false;
            // refrescamos el dasm del otro jugador por si fue cambiado por la ejecucion del jugador actual
            players[otherplayer].actual.dasm = otherdasm;

            //players[thisplayer].actual.dasm = dasm;
            // Actualizamos los ciclos y registros 
            players[this.uidx].user = userregs;
            if (players[this.uidx].actual.cycles == -1)
                players[this.uidx].actual.cyclesfixed = ciclos;
            players[this.uidx].actual.cycles = ciclos;
            // guardamos el antiguo puntero a instruccion 
            players[thisplayer].actual.oldpc = players[thisplayer].actual.ipc();
            players[thisplayer].actual.oldpcsize = players[thisplayer].actual.pcsize;

            // Actualizamos la instruccion actual
            players[thisplayer].actual.pc = PC[0];
            players[thisplayer].actual.pcsize = ipcsize;

            players[thisplayer].actual.ins = newins;
            players[thisplayer].actual.mem = mem;
            players[thisplayer].actual.dasm = dasm;
            players[thisplayer].actual.regs = regs1;
            return players[thisplayer].actual.dead;
        }
        public string formatregs(string regs)
        {
            string[] reg = regs.Split('\n');
            string regformat = "";
            int x = 0;
            foreach (string s in reg)
            {
                if (s.StartsWith("oeax") == false)
                {
                    if (x % 2 == 0 && x != 0)
                        regformat += "\n";
                    regformat += s + " ";
                    x++;
                }
            }
            return regformat;
        }
    }
}

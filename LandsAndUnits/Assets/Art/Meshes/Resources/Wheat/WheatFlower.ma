//Maya ASCII 2019 scene
//Name: WheatFlower.ma
//Last modified: Mon, Mar 22, 2021 01:04:57 AM
//Codeset: 1252
requires maya "2019";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2019";
fileInfo "version" "2019";
fileInfo "cutIdentifier" "201812112215-434d8d9c04";
fileInfo "osv" "Microsoft Windows 10 Technical Preview  (Build 19041)\n";
fileInfo "license" "student";
createNode transform -n "polySurface7";
	rename -uid "31BCC672-4CFC-B7AA-ADD3-D099FD407D06";
	setAttr ".rp" -type "double3" -5.7386708112881069 0.00450848788022995 -3.7252902984619141e-09 ;
	setAttr ".sp" -type "double3" -5.7386708112881069 0.00450848788022995 -3.7252902984619141e-09 ;
createNode mesh -n "polySurfaceShape3" -p "polySurface7";
	rename -uid "4A85F1CA-4FC4-A28F-08A4-8C8FBAA85C80";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr ".iog[0].og[0].gcl" -type "componentList" 1 "f[0:67]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.91071182489395142 0.13375523686408997 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 85 ".uvst[0].uvsp[0:84]" -type "float2" 0.0025897622 0.01603511
		 0.0025897622 0.01603511 0.052237511 0.018326789 0.052237511 0.018326789 0.0025897622
		 0.043426633 0.052237511 0.043426633 0.0025897622 0.070818186 0.052237511 0.068526506
		 0.0025897622 0.070818186 0.052237511 0.068526506 0.0025897622 0.043426633 0.052237511
		 0.043426633 0.17259663 0.024183631 0.17259663 0.024183631 0.17259663 0.043426633
		 0.17259663 0.062669635 0.17259663 0.062669635 0.17259663 0.043426633 0.34705204 0.024864912
		 0.35024637 0.024864793 0.34545493 0.043426633 0.34705204 0.061988354 0.35024637 0.061988354
		 0.35184425 0.043426633 0.5212326 0.028360844 0.52413988 0.028360963 0.51977944 0.043426394
		 0.5212326 0.058491826 0.52413988 0.058491707 0.52559328 0.043426275 0.82295543 0.031527042
		 0.81697732 0.031527042 0.89129394 0.033633888 0.89706057 0.033634007 0.81398767 0.043425918
		 0.88841039 0.043425739 0.81697732 0.055324912 0.89129394 0.05321753 0.82295567 0.055324793
		 0.89706069 0.05321753 0.82594615 0.043425918 0.89994484 0.04342562 0.94219345 0.035097659
		 0.94736439 0.035097778 0.93960887 0.043426454 0.94219345 0.05175519 0.94736439 0.051755071
		 0.94994956 0.043426335 0.99692166 0.043426633 0.63529378 0.030132174 0.76200134 0.031367421
		 0.76425344 0.043425918 0.6380139 0.043426156 0.6302622 0.030116439 0.75744504 0.031369567
		 0.62778085 0.043426275 0.75498348 0.043426156 0.6302622 0.056736112 0.75744504 0.055482745
		 0.63529378 0.056720257 0.76200134 0.055484653 0.91071177 0.073483765 0.91071182 0.19402671
		 0.84330344 0.083352506 0.84330344 0.18415788 0.76251554 0.12065434 0.76251554 0.14685616
		 0.91071182 0.073483765 0.91071182 0.19402671 0.84330344 0.083352506 0.8433035 0.18415794
		 0.76251554 0.12065434 0.76251554 0.14685616 0.91071188 0.073483825 0.91071188 0.19402668
		 0.8433035 0.083352566 0.8433035 0.18415794 0.7625156 0.12065434 0.7625156 0.14685616
		 0.91071188 0.073483825 0.91071188 0.19402668 0.8433035 0.083352566 0.8433035 0.18415794
		 0.7625156 0.12065434 0.7625156 0.14685616;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 85 ".vt[0:84]"  -5.91362762 0.61339909 0.047693059 -5.82756901 0.53251529 0.010366803
		 -5.87086391 0.58037305 0.039883845 -5.91362762 0.61339909 -0.047693066 -5.82756901 0.53251529 -0.01036681
		 -5.87086391 0.58037305 -0.039883863 -5.83849669 0.52225417 0.00081078755 -5.8162694 0.54071444 0.00081077527
		 -5.94041729 0.58074766 0.00081080158 -5.88661051 0.64355886 0.00081076124 -5.89557886 0.553433 0.00081079296
		 -5.84603977 0.60478306 0.00081076985 -5.91362762 0.61233085 0.047693059 -5.82756901 0.53144699 0.010366803
		 -5.87086391 0.57930475 0.039883833 -5.91362762 0.61233085 -0.047693066 -5.82756901 0.53144699 -0.01036681
		 -5.87086391 0.57930475 -0.039883863 -5.83849669 0.52225417 -3.8912322e-09 -5.8162694 0.54071444 -1.6125732e-08
		 -5.94041729 0.58074766 1.0143259e-08 -5.88661051 0.64355886 -3.0160219e-08 -5.89557886 0.553433 1.5732748e-09
		 -5.84603977 0.60478306 -2.1590242e-08 -5.73233795 0.0045084879 -0.012580223 -5.74686432 0.0045084879 -0.012580226
		 -5.7541275 0.0045084879 -2.1956466e-09 -5.74686432 0.0045084879 0.012580224 -5.73233795 0.0045084879 0.012580225
		 -5.72507238 0.0045084879 -3.1046044e-11 -5.73294544 0.036478128 -0.011527692 -5.74625635 0.036478128 -0.011527695
		 -5.75291109 0.036478128 -2.0499027e-09 -5.74625635 0.036478128 0.011527693 -5.73294544 0.036478128 0.011527694
		 -5.72628784 0.036478128 -6.6404174e-11 -5.7344985 0.11398095 -0.0088377884 -5.74470329 0.11398095 -0.0088377902
		 -5.74980593 0.11398095 -1.6642064e-09 -5.74470329 0.11398095 0.0088377884 -5.7344985 0.11398095 0.0088377893
		 -5.72939444 0.11398095 -1.4354336e-10 -5.73608589 0.22837487 -0.0085249348 -5.7457118 0.22631794 -0.0085248994
		 -5.75052595 0.22528954 -5.1146829e-09 -5.7457118 0.22631797 0.0085248891 -5.73608589 0.2283749 0.008524918
		 -5.73127174 0.22940379 -2.2009322e-08 -5.74906158 0.34035006 -0.0069192736 -5.75682831 0.33847803 -0.006919302
		 -5.76071215 0.33754227 -1.2432841e-07 -5.75682831 0.33847803 0.0069190613 -5.74906158 0.34035009 0.0069190124
		 -5.74517679 0.34128603 -1.3803673e-07 -5.82575512 0.53276628 -0.0054651429 -5.83071995 0.52891678 -0.0054651666
		 -5.83320379 0.52699167 -3.0208739e-07 -5.83071995 0.52891678 0.0054645673 -5.82575512 0.53276646 0.0054644989
		 -5.82327175 0.53469205 -3.448998e-07 -5.86937714 0.58048475 -0.0044974633 -5.87298059 0.57677144 -0.0044974876
		 -5.87478399 0.57491463 -3.7348556e-07 -5.87298059 0.57677144 0.0044966978 -5.86937714 0.58048481 0.0044966824
		 -5.86757517 0.58234203 -4.1731295e-07 -5.91199112 0.61287689 -0.0038251728 -5.91488695 0.60954714 -0.0038252131
		 -5.91633511 0.60788292 -5.7166158e-08 -5.91488695 0.60954714 0.0038250752 -5.91199064 0.61287689 0.0038250182
		 -5.91054296 0.61454159 -1.0671016e-07 -5.97442436 0.64843816 5.3957855e-10 -5.7668786 0.41192541 -0.0061057894
		 -5.80133533 0.49351609 -0.0055384669 -5.77321863 0.40868539 -0.0061129956 -5.80699778 0.49058217 -0.0055375122
		 -5.77640104 0.40708756 -1.5410244e-07 -5.80975437 0.48899707 -2.3749058e-07 -5.77321863 0.40868542 0.0061126943
		 -5.80699778 0.49058217 0.0055370443 -5.7668786 0.41192541 0.0061054053 -5.80133533 0.49351612 0.0055379178
		 -5.76374865 0.41367689 -2.2785154e-07 -5.79849195 0.49496633 -3.1256613e-07;
	setAttr -s 148 ".ed[0:147]"  3 0 0 4 1 0 5 2 1 2 0 0 1 2 0 5 3 0 4 5 0
		 6 10 0 7 11 0 6 7 0 8 9 0 10 8 0 11 9 0 10 11 1 15 12 0 16 13 0 17 14 1 14 12 0 13 14 0
		 17 15 0 16 17 0 18 22 0 19 23 0 18 19 0 20 21 0 22 20 0 23 21 0 22 23 1 24 25 0 25 26 0
		 26 27 0 27 28 0 28 29 0 29 24 0 30 31 0 31 32 0 32 33 0 33 34 0 34 35 0 35 30 0 24 30 0
		 25 31 0 26 32 0 27 33 0 28 34 0 29 35 0 30 36 0 31 37 0 36 37 1 32 38 0 37 38 1 33 39 0
		 38 39 1 34 40 0 39 40 1 35 41 0 40 41 1 41 36 1 36 42 0 37 43 0 42 43 1 38 44 0 43 44 1
		 39 45 0 44 45 1 40 46 0 45 46 1 41 47 0 46 47 1 47 42 1 42 48 0 43 49 0 48 49 1 44 50 0
		 49 50 1 45 51 0 50 51 1 46 52 0 51 52 1 47 53 0 52 53 1 53 48 1 54 55 1 55 56 1 56 57 1
		 57 58 1 58 59 1 59 54 1 54 60 0 55 61 0 60 61 0 56 62 0 61 62 0 57 63 0 62 63 0 58 64 0
		 63 64 0 59 65 0 64 65 0 65 60 0 60 66 0 61 67 0 66 67 0 62 68 0 67 68 0 63 69 0 68 69 0
		 64 70 0 69 70 0 65 71 0 70 71 0 71 66 0 66 72 0 67 72 0 68 72 0 69 72 0 70 72 0 71 72 0
		 73 74 0 74 84 1 84 83 0 83 73 1 73 75 1 75 76 0 76 74 1 75 77 1 77 78 0 78 76 1 77 79 1
		 79 80 0 80 78 1 79 81 1 81 82 0 82 80 1 81 83 1 84 82 1 49 75 0 73 48 0 50 77 0 51 79 0
		 52 81 0 53 83 0 76 55 0 54 74 0 78 56 0 80 57 0 82 58 0 84 59 0;
	setAttr -s 68 -ch 266 ".fc[0:67]" -type "polyFaces" 
		f 4 5 0 -4 -3
		mu 0 4 75 73 74 76
		f 4 6 2 -5 -2
		mu 0 4 77 75 76 78
		f 4 13 12 -11 -12
		mu 0 4 63 64 62 61
		f 4 9 8 -14 -8
		mu 0 4 65 66 64 63
		f 4 16 17 -15 -20
		mu 0 4 81 82 80 79
		f 4 15 18 -17 -21
		mu 0 4 83 84 82 81
		f 4 25 24 -27 -28
		mu 0 4 69 67 68 70
		f 4 21 27 -23 -24
		mu 0 4 71 69 70 72
		f 4 28 41 -35 -41
		mu 0 4 0 1 2 3
		f 4 29 42 -36 -42
		mu 0 4 1 4 5 2
		f 4 30 43 -37 -43
		mu 0 4 4 6 7 5
		f 4 31 44 -38 -44
		mu 0 4 6 8 9 7
		f 4 32 45 -39 -45
		mu 0 4 8 10 11 9
		f 4 33 40 -40 -46
		mu 0 4 10 0 3 11
		f 4 34 47 -49 -47
		mu 0 4 3 2 12 13
		f 4 35 49 -51 -48
		mu 0 4 2 5 14 12
		f 4 36 51 -53 -50
		mu 0 4 5 7 15 14
		f 4 37 53 -55 -52
		mu 0 4 7 9 16 15
		f 4 38 55 -57 -54
		mu 0 4 9 11 17 16
		f 4 39 46 -58 -56
		mu 0 4 11 3 13 17
		f 4 48 59 -61 -59
		mu 0 4 13 12 18 19
		f 4 50 61 -63 -60
		mu 0 4 12 14 20 18
		f 4 52 63 -65 -62
		mu 0 4 14 15 21 20
		f 4 54 65 -67 -64
		mu 0 4 15 16 22 21
		f 4 56 67 -69 -66
		mu 0 4 16 17 23 22
		f 4 57 58 -70 -68
		mu 0 4 17 13 19 23
		f 4 60 71 -73 -71
		mu 0 4 19 18 24 25
		f 4 62 73 -75 -72
		mu 0 4 18 20 26 24
		f 4 64 75 -77 -74
		mu 0 4 20 21 27 26
		f 4 66 77 -79 -76
		mu 0 4 21 22 28 27
		f 4 68 79 -81 -78
		mu 0 4 22 23 29 28
		f 4 69 70 -82 -80
		mu 0 4 23 19 25 29
		f 4 82 89 -91 -89
		mu 0 4 30 31 32 33
		f 4 83 91 -93 -90
		mu 0 4 31 34 35 32
		f 4 84 93 -95 -92
		mu 0 4 34 36 37 35
		f 4 85 95 -97 -94
		mu 0 4 36 38 39 37
		f 4 86 97 -99 -96
		mu 0 4 38 40 41 39
		f 4 87 88 -100 -98
		mu 0 4 40 30 33 41
		f 4 90 101 -103 -101
		mu 0 4 33 32 42 43
		f 4 92 103 -105 -102
		mu 0 4 32 35 44 42
		f 4 94 105 -107 -104
		mu 0 4 35 37 45 44
		f 4 96 107 -109 -106
		mu 0 4 37 39 46 45
		f 4 98 109 -111 -108
		mu 0 4 39 41 47 46
		f 4 99 100 -112 -110
		mu 0 4 41 33 43 47
		f 3 102 113 -113
		mu 0 3 43 42 48
		f 3 104 114 -114
		mu 0 3 42 44 48
		f 3 106 115 -115
		mu 0 3 44 45 48
		f 3 108 116 -116
		mu 0 3 45 46 48
		f 3 110 117 -117
		mu 0 3 46 47 48
		f 3 111 112 -118
		mu 0 3 47 43 48
		f 4 118 119 120 121
		mu 0 4 49 50 51 52
		f 4 -119 122 123 124
		mu 0 4 50 49 53 54
		f 4 -124 125 126 127
		mu 0 4 54 53 55 56
		f 4 -127 128 129 130
		mu 0 4 56 55 57 58
		f 4 -130 131 132 133
		mu 0 4 58 57 59 60
		f 4 -133 134 -121 135
		mu 0 4 60 59 52 51
		f 4 72 136 -123 137
		mu 0 4 25 24 53 49
		f 4 74 138 -126 -137
		mu 0 4 24 26 55 53
		f 4 76 139 -129 -139
		mu 0 4 26 27 57 55
		f 4 78 140 -132 -140
		mu 0 4 27 28 59 57
		f 4 80 141 -135 -141
		mu 0 4 28 29 52 59
		f 4 81 -138 -122 -142
		mu 0 4 29 25 49 52
		f 4 -125 142 -83 143
		mu 0 4 50 54 31 30
		f 4 -128 144 -84 -143
		mu 0 4 54 56 34 31
		f 4 -131 145 -85 -145
		mu 0 4 56 58 36 34
		f 4 -134 146 -86 -146
		mu 0 4 58 60 38 36
		f 4 -136 147 -87 -147
		mu 0 4 60 51 40 38
		f 4 -120 -144 -88 -148
		mu 0 4 51 50 30 40;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".pd[0]" -type "dataPolyComponent" Index_Data UV 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "polySurface8" -p "polySurface7";
	rename -uid "4D09F361-4002-41B3-5895-ABB42B085DDE";
createNode transform -n "transform2" -p "polySurface8";
	rename -uid "92068650-44A2-40CF-2360-978AE0AD22EE";
	setAttr ".v" no;
createNode mesh -n "polySurfaceShape4" -p "transform2";
	rename -uid "4CBE86F6-4181-84F0-7A85-EB99F9C424AF";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr -s 2 ".iog[0].og";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode transform -n "polySurface9" -p "polySurface7";
	rename -uid "28D1D020-4BFC-66DB-FCF2-F4BD238F636B";
createNode transform -n "transform3" -p "polySurface9";
	rename -uid "F7DD6C10-443A-4D66-9ACF-23B49F94339D";
	setAttr ".v" no;
createNode mesh -n "polySurfaceShape5" -p "transform3";
	rename -uid "5B0F008F-4435-BADD-E4DD-28A89E99FB01";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr -s 2 ".iog[0].og";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode transform -n "polySurface10" -p "polySurface7";
	rename -uid "28E667AA-4CFE-C2DB-CF64-A0A0D14806AC";
createNode transform -n "transform5" -p "|polySurface7|polySurface10";
	rename -uid "6BB1DC4E-43E3-63DD-EAD6-DFA3A1E6575F";
	setAttr ".v" no;
createNode mesh -n "polySurfaceShape6" -p "transform5";
	rename -uid "7472DA42-4D4B-1DD2-520D-368CA9AF6CFF";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr -s 2 ".iog[0].og";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode transform -n "polySurface11" -p "polySurface7";
	rename -uid "0179DA77-4F9B-B71C-E349-4D848E0BE39E";
createNode transform -n "transform4" -p "polySurface11";
	rename -uid "8E3B5FDD-4712-E90B-0E93-E2BC36195BD8";
	setAttr ".v" no;
createNode mesh -n "polySurfaceShape7" -p "transform4";
	rename -uid "748DCCC2-4E48-6622-6F45-9889ED64DEE0";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr -s 2 ".iog[0].og";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode transform -n "transform1" -p "polySurface7";
	rename -uid "667FB901-4DAB-77B8-DF34-C5AC1FF898FA";
	setAttr ".v" no;
createNode mesh -n "polySurface7Shape" -p "transform1";
	rename -uid "7901F8D0-4F33-85F7-49CA-C4B03317CC5E";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr -s 2 ".iog[0].og";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.81532266736030579 0.65561461448669434 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode transform -n "polySurface10";
	rename -uid "7646D954-4467-2BA9-E620-9D993C843C5D";
	setAttr ".rp" -type "double3" -5.8290014679860143 0.53281307853034954 -3.7252902984619141e-09 ;
	setAttr ".sp" -type "double3" -5.8290014679860143 0.53281307853034954 -3.7252902984619141e-09 ;
createNode mesh -n "polySurface10Shape" -p "|polySurface10";
	rename -uid "6A17DE5F-436A-CBDC-2F71-C39405F883B8";
	setAttr -k off ".v";
	setAttr -s 2 ".iog[0].og";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode transform -s -n "persp";
	rename -uid "23BAC597-4F8E-4909-6EEB-E58A476B2EA8";
	setAttr ".v" no;
	setAttr ".t" -type "double3" -5.8281081391876333 0.56007815753939361 -0.7627161439882697 ;
	setAttr ".r" -type "double3" -0.93835272959536709 -183.39999999999534 0 ;
createNode camera -s -n "perspShape" -p "persp";
	rename -uid "74FFF005-4047-3CF9-CE14-1D8196E9E6E2";
	setAttr -k off ".v" no;
	setAttr ".fl" 34.999999999999993;
	setAttr ".coi" 0.68804746711977594;
	setAttr ".imn" -type "string" "persp";
	setAttr ".den" -type "string" "persp_depth";
	setAttr ".man" -type "string" "persp_mask";
	setAttr ".tp" -type "double3" -5.9083206653594971 0.60221737623214722 -3.7252902984619141e-09 ;
	setAttr ".hc" -type "string" "viewSet -p %camera";
createNode transform -s -n "top";
	rename -uid "8EB53FB7-4EBD-9237-1CD8-E28DF9242382";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 0 1000.1 0 ;
	setAttr ".r" -type "double3" -90 0 0 ;
createNode camera -s -n "topShape" -p "top";
	rename -uid "3763D16E-4816-39FD-67EE-EFB20E24EA29";
	setAttr -k off ".v" no;
	setAttr ".rnd" no;
	setAttr ".coi" 1000.1;
	setAttr ".ow" 30;
	setAttr ".imn" -type "string" "top";
	setAttr ".den" -type "string" "top_depth";
	setAttr ".man" -type "string" "top_mask";
	setAttr ".hc" -type "string" "viewSet -t %camera";
	setAttr ".o" yes;
createNode transform -s -n "front";
	rename -uid "DA043663-4A68-FF87-5D29-20A7E5ED03E0";
	setAttr ".v" no;
	setAttr ".t" -type "double3" -5.9083206653594971 0.60221737623214722 1000.1176335406016 ;
createNode camera -s -n "frontShape" -p "front";
	rename -uid "6B04FAB0-451F-738E-66D0-499CB9761C31";
	setAttr -k off ".v" no;
	setAttr ".rnd" no;
	setAttr ".coi" 1000.1176335443268;
	setAttr ".ow" 0.62067271944750468;
	setAttr ".imn" -type "string" "front";
	setAttr ".den" -type "string" "front_depth";
	setAttr ".man" -type "string" "front_mask";
	setAttr ".tp" -type "double3" -5.9083206653594971 0.60221737623214722 -3.7252902984619141e-09 ;
	setAttr ".hc" -type "string" "viewSet -f %camera";
	setAttr ".o" yes;
createNode transform -s -n "side";
	rename -uid "0C07290C-4BE9-EA52-240F-AEB4740742B1";
	setAttr ".v" no;
	setAttr ".t" -type "double3" 1000.1 0 0 ;
	setAttr ".r" -type "double3" 0 90 0 ;
createNode camera -s -n "sideShape" -p "side";
	rename -uid "5F191BB4-4BB3-33E3-9D7B-CAA0E6BBF0BC";
	setAttr -k off ".v" no;
	setAttr ".rnd" no;
	setAttr ".coi" 1000.1;
	setAttr ".ow" 30;
	setAttr ".imn" -type "string" "side";
	setAttr ".den" -type "string" "side_depth";
	setAttr ".man" -type "string" "side_mask";
	setAttr ".hc" -type "string" "viewSet -s %camera";
	setAttr ".o" yes;
createNode groupParts -n "groupParts7";
	rename -uid "39F9ADE9-4CC3-0168-4F51-4389CEC8EFDA";
	setAttr ".ihi" 0;
	setAttr ".ic" -type "componentList" 1 "f[0:11]";
createNode polyUnite -n "polyUnite1";
	rename -uid "81507D9E-46FC-E4D0-53F8-3E8334C98065";
	setAttr -s 4 ".ip";
	setAttr -s 4 ".im";
createNode groupParts -n "groupParts4";
	rename -uid "B409BF7C-4DB9-91B1-EA64-FDA60EA52070";
	setAttr ".ihi" 0;
	setAttr ".ic" -type "componentList" 1 "f[0:2]";
createNode polySeparate -n "polySeparate1";
	rename -uid "0D9B7186-45DF-065D-7EFE-CD9CDD945CFA";
	setAttr ".ic" 5;
	setAttr -s 4 ".out";
createNode polyChipOff -n "polyChipOff1";
	rename -uid "BE1695D9-47E5-6879-1362-3AAF5415A58C";
	setAttr ".ics" -type "componentList" 2 "f[0:7]" "f[68:71]";
	setAttr ".ix" -type "matrix" 1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1;
	setAttr ".ws" yes;
	setAttr ".rs" 64179;
	setAttr ".dup" no;
createNode polyTweakUV -n "polyTweakUV1";
	rename -uid "B7E8B68A-47ED-5AA5-CF96-02958065E2EA";
	setAttr ".uopa" yes;
	setAttr -s 93 ".uvtk[0:92]" -type "float2" 0.051981807 -0.015975382
		 0.051981807 -0.015975382 0.048341811 0.032193556 0.048341811 0.032193556 0.025370169
		 -0.016755268 0.023956612 0.031478912 -0.0012414921 -0.017535161 -0.00042863097 0.030764293
		 -0.0012414921 -0.017535161 -0.00042863097 0.030764293 0.025370169 -0.016755268 0.023956612
		 0.031478912 0.039224885 0.14895912 0.039224885 0.14895912 0.020529777 0.14841121
		 0.0018346757 0.14786331 0.0018346757 0.14786331 0.020529777 0.14841121 0.033595979
		 0.31842804 0.033505112 0.32153144 0.015608177 0.31634793 -0.0024704784 0.3173711
		 -0.0025614351 0.32047439 0.015426233 0.3225553 0.025240302 0.48754978 0.025157392
		 0.49037433 0.010645032 0.48570907 -0.0040327311 0.48669189 -0.0041154027 0.48951638
		 0.010479629 0.49135733 0.013573706 0.78059185 0.013743937 0.77478385 0.0095810294
		 0.8469246 0.0094168186 0.85252696 0.0022688508 0.7715407 0.0001500845 0.84384441
		 -0.0093764067 0.77410638 -0.009444952 0.84636718 -0.0095465779 0.77991456 -0.0096090436
		 0.8519696 0.0019283295 0.78315866 -0.00017815828 0.8550505 0.0067099333 0.89633334
		 0.006562531 0.90135711 -0.001308322 0.89358532 -0.009473443 0.89585894 -0.0096204877
		 0.90088272 -0.0016025305 0.90363151 -0.0029402375 0.94926608 0.020271897 0.59831309
		 0.015464306 0.72137785 0.0036848187 0.72322237 0.0072789788 0.60057729 0.020430446
		 0.59342515 0.015591979 0.71695113 0.0075701475 0.59063554 0.0039486289 0.71421653
		 -0.0054312944 0.59266722 -0.0078347325 0.71626467 -0.005559206 0.59755611 -0.00796628
		 0.72069108 0.69537157 0.29382101 0.44347507 0.54013628 0.39612496 0.50558865 0.70167786
		 0.22778547 0.66439974 0.41198224 0.58316785 0.50482398 0.38174933 0.42177165 0.70774096
		 0.14766534 -0.10445151 0.29382089 0.070217229 0.22778547 0.15181921 0.50558865 -0.039836671
		 0.5401361 0.27345914 0.14766534 0.37549961 0.42177165 -0.32760245 0.41198224 -0.29504967
		 0.5048241 0.094033577 0.18551274 0.14547528 -0.012271672 0.5104304 0.099703714 0.39923155
		 0.27915367 0.11455781 0.44739199 0.19388618 0.47173166 0.094033577 0.18894596 0.39923143
		 0.2825869 0.5104304 0.10313694 0.14547528 -0.0088385046 0.11455781 0.45082521 0.19388629
		 0.47516477 0.25012785 -0.24908036 0.61508274 -0.13710502 0.61508286 -0.13687232 0.25012785
		 -0.24884766;
createNode polyPlanarProj -n "polyPlanarProj1";
	rename -uid "AA83BAA8-409B-61F4-7A7C-05B2F2D69D60";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 4 "f[0:1]" "f[4:5]" "f[68]" "f[70]";
	setAttr ".ix" -type "matrix" 1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1;
	setAttr ".ws" yes;
	setAttr ".pc" -type "double3" -5.9007222652435303 0.59003174304962158 -3.7252902984619141e-09 ;
	setAttr ".ro" -type "double3" -50 90 0 ;
	setAttr ".ps" -type "double2" 0.2 0.2 ;
	setAttr ".cam" -type "matrix" 1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1;
createNode polyAutoProj -n "polyAutoProj1";
	rename -uid "A99AC388-4375-9FA5-5728-07B1B65C778E";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 2 "f[0:7]" "f[68:71]";
	setAttr ".ix" -type "matrix" 1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1;
	setAttr ".s" -type "double3" 0.18410253524780273 0.18410253524780273 0.18410253524780273 ;
	setAttr ".ps" 0.20000000298023224;
	setAttr ".dl" yes;
createNode polyTweak -n "polyTweak2";
	rename -uid "05BE0CB0-45F5-BB30-6266-9DB9D2D8AD3E";
	setAttr ".uopa" yes;
	setAttr -s 4 ".tk";
	setAttr ".tk[87]" -type "float3" -0.0072697876 -0.0099748252 0 ;
	setAttr ".tk[88]" -type "float3" 0.0071007218 0.0096366964 0 ;
	setAttr ".tk[91]" -type "float3" -0.0072697876 -0.0099748252 0 ;
	setAttr ".tk[92]" -type "float3" 0.0071007218 0.0096366964 0 ;
createNode polySoftEdge -n "polySoftEdge1";
	rename -uid "AC274C9A-4E9A-2645-4BE3-948037F85410";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 2 "e[0]" "e[14]";
	setAttr ".ix" -type "matrix" 1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1;
	setAttr ".a" 180;
createNode polyTweak -n "polyTweak1";
	rename -uid "6B61D132-4443-93FD-C470-ABB0107C0FA3";
	setAttr ".uopa" yes;
	setAttr -s 8 ".tk[85:92]" -type "float3"  -0.059597556 0.035217423 2.0489097e-08
		 -0.059597556 0.035217423 -2.0489097e-08 -0.052684754 0.041964844 2.910383e-11 -0.068254665
		 0.028985018 -5.8207661e-11 -0.060247686 0.035438232 2.0489097e-08 -0.060247686 0.035438232
		 -2.0489097e-08 -0.052684754 0.041964844 7.1054274e-15 -0.068254665 0.028985018 1.2434498e-14;
createNode polyExtrudeEdge -n "polyExtrudeEdge1";
	rename -uid "68303C10-40DF-06D7-1470-84856C4CC28A";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 4 "e[0]" "e[10]" "e[14]" "e[24]";
	setAttr ".ix" -type "matrix" 1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1;
	setAttr ".ws" yes;
	setAttr ".pvt" -type "float3" -5.9135141 0.61215329 -3.7252903e-09 ;
	setAttr ".rs" 39147;
	setAttr ".c[0]"  0 1 1;
	setAttr ".cbn" -type "double3" -5.9404172897338867 0.58074766397476196 -0.047693066298961639 ;
	setAttr ".cbx" -type "double3" -5.8866105079650879 0.64355885982513428 0.047693058848381042 ;
createNode groupParts -n "groupParts1";
	rename -uid "CA9F254D-4638-1BB7-5EF9-489E7450EDC2";
	setAttr ".ihi" 0;
	setAttr ".ic" -type "componentList" 1 "f[0:67]";
createNode groupId -n "groupId23";
	rename -uid "41E5A2C6-488D-F359-745B-3E8B2036F759";
	setAttr ".ihi" 0;
createNode groupId -n "groupId26";
	rename -uid "08BED803-40A0-5E84-4122-41AC86E9FEB7";
	setAttr ".ihi" 0;
createNode groupParts -n "groupParts5";
	rename -uid "1321EE31-4867-3B46-EBB4-9699082ED076";
	setAttr ".ihi" 0;
	setAttr ".ic" -type "componentList" 1 "f[0:2]";
createNode groupId -n "groupId27";
	rename -uid "C293F85B-43B1-21AC-5D01-558CA3D62182";
	setAttr ".ihi" 0;
createNode groupParts -n "groupParts3";
	rename -uid "63698465-4F4A-F91E-D5F8-04A7378522BF";
	setAttr ".ihi" 0;
	setAttr ".ic" -type "componentList" 1 "f[0:2]";
createNode groupId -n "groupId25";
	rename -uid "6FE41C74-4342-D878-EAB0-6DAEB6075028";
	setAttr ".ihi" 0;
createNode groupParts -n "groupParts2";
	rename -uid "B0A8C03B-4895-BD98-6EC2-EC9C9E541CBB";
	setAttr ".ihi" 0;
	setAttr ".ic" -type "componentList" 1 "f[0:2]";
createNode groupId -n "groupId24";
	rename -uid "5A69DC23-459F-AD6C-994C-FCAC47D9FB67";
	setAttr ".ihi" 0;
createNode groupId -n "groupId29";
	rename -uid "4AC555D0-4488-3F3E-B2B1-7D938ABCFA97";
	setAttr ".ihi" 0;
createNode lightLinker -s -n "lightLinker1";
	rename -uid "056251F5-452C-18CF-01E4-67B86CE27D18";
	setAttr -s 2 ".lnk";
	setAttr -s 2 ".slnk";
createNode shapeEditorManager -n "shapeEditorManager";
	rename -uid "EA76F787-4FA3-9DE5-61B1-31BDF8623F82";
createNode poseInterpolatorManager -n "poseInterpolatorManager";
	rename -uid "BAC2C79E-466A-6A27-05AC-1D849F17F300";
createNode displayLayerManager -n "layerManager";
	rename -uid "9CD0D446-4100-34E9-61FC-F29A43FFA0AE";
createNode displayLayer -n "defaultLayer";
	rename -uid "0D33185E-421E-6210-32E5-C5BF3348066E";
createNode renderLayerManager -n "renderLayerManager";
	rename -uid "9C0B9151-4720-8EEE-0DEF-D5B1C8BF3C5F";
createNode renderLayer -n "defaultRenderLayer";
	rename -uid "A474D008-4612-04A4-E17F-DB88E57FAAFE";
	setAttr ".g" yes;
createNode script -n "uiConfigurationScriptNode";
	rename -uid "D8D9111F-4313-F36D-C028-BEB8E26E3B6D";
	setAttr ".b" -type "string" (
		"// Maya Mel UI Configuration File.\n//\n//  This script is machine generated.  Edit at your own risk.\n//\n//\n\nglobal string $gMainPane;\nif (`paneLayout -exists $gMainPane`) {\n\n\tglobal int $gUseScenePanelConfig;\n\tint    $useSceneConfig = $gUseScenePanelConfig;\n\tint    $nodeEditorPanelVisible = stringArrayContains(\"nodeEditorPanel1\", `getPanel -vis`);\n\tint    $nodeEditorWorkspaceControlOpen = (`workspaceControl -exists nodeEditorPanel1Window` && `workspaceControl -q -visible nodeEditorPanel1Window`);\n\tint    $menusOkayInPanels = `optionVar -q allowMenusInPanels`;\n\tint    $nVisPanes = `paneLayout -q -nvp $gMainPane`;\n\tint    $nPanes = 0;\n\tstring $editorName;\n\tstring $panelName;\n\tstring $itemFilterName;\n\tstring $panelConfig;\n\n\t//\n\t//  get current state of the UI\n\t//\n\tsceneUIReplacement -update $gMainPane;\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Top View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Top View\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"top\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 32768\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n"
		+ "            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -controllers 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n"
		+ "            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 934\n            -height 375\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Side View\")) `;\n"
		+ "\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Side View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"side\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 32768\n            -fogging 0\n"
		+ "            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -controllers 1\n            -nurbsCurves 1\n"
		+ "            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 933\n            -height 375\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Front View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Front View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"front\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n"
		+ "            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 32768\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n"
		+ "            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -controllers 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n"
		+ "            -width 1878\n            -height 798\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"modelPanel\" (localizedPanelLabel(\"Persp View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tmodelPanel -edit -l (localizedPanelLabel(\"Persp View\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        modelEditor -e \n            -camera \"persp\" \n            -useInteractiveMode 0\n            -displayLights \"default\" \n            -displayAppearance \"smoothShaded\" \n            -activeOnly 0\n            -ignorePanZoom 0\n            -wireframeOnShaded 0\n            -headsUpDisplay 1\n            -holdOuts 1\n            -selectionHiliteDisplay 1\n            -useDefaultMaterial 0\n            -bufferMode \"double\" \n            -twoSidedLighting 0\n            -backfaceCulling 0\n            -xray 0\n            -jointXray 0\n"
		+ "            -activeComponentsXray 0\n            -displayTextures 0\n            -smoothWireframe 0\n            -lineWidth 1\n            -textureAnisotropic 0\n            -textureHilight 1\n            -textureSampling 2\n            -textureDisplay \"modulate\" \n            -textureMaxSize 32768\n            -fogging 0\n            -fogSource \"fragment\" \n            -fogMode \"linear\" \n            -fogStart 0\n            -fogEnd 100\n            -fogDensity 0.1\n            -fogColor 0.5 0.5 0.5 1 \n            -depthOfFieldPreview 1\n            -maxConstantTransparency 1\n            -rendererName \"vp2Renderer\" \n            -objectFilterShowInHUD 1\n            -isFiltered 0\n            -colorResolution 256 256 \n            -bumpResolution 512 512 \n            -textureCompression 0\n            -transparencyAlgorithm \"frontAndBackCull\" \n            -transpInShadows 0\n            -cullingOverride \"none\" \n            -lowQualityLighting 0\n            -maximumNumHardwareLights 1\n            -occlusionCulling 0\n            -shadingModel 0\n"
		+ "            -useBaseRenderer 0\n            -useReducedRenderer 0\n            -smallObjectCulling 0\n            -smallObjectThreshold -1 \n            -interactiveDisableShadows 0\n            -interactiveBackFaceCull 0\n            -sortTransparent 1\n            -controllers 1\n            -nurbsCurves 1\n            -nurbsSurfaces 1\n            -polymeshes 1\n            -subdivSurfaces 1\n            -planes 1\n            -lights 1\n            -cameras 1\n            -controlVertices 1\n            -hulls 1\n            -grid 1\n            -imagePlane 1\n            -joints 1\n            -ikHandles 1\n            -deformers 1\n            -dynamics 1\n            -particleInstancers 1\n            -fluids 1\n            -hairSystems 1\n            -follicles 1\n            -nCloths 1\n            -nParticles 1\n            -nRigids 1\n            -dynamicConstraints 1\n            -locators 1\n            -manipulators 1\n            -pluginShapes 1\n            -dimensions 1\n            -handles 1\n            -pivots 1\n            -textures 1\n"
		+ "            -strokes 1\n            -motionTrails 1\n            -clipGhosts 1\n            -greasePencils 1\n            -shadows 0\n            -captureSequenceNumber -1\n            -width 933\n            -height 375\n            -sceneRenderFilter 0\n            $editorName;\n        modelEditor -e -viewSelected 0 $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"outlinerPanel\" (localizedPanelLabel(\"ToggledOutliner\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\toutlinerPanel -edit -l (localizedPanelLabel(\"ToggledOutliner\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        outlinerEditor -e \n            -showShapes 0\n            -showAssignedMaterials 0\n            -showTimeEditor 1\n            -showReferenceNodes 1\n            -showReferenceMembers 1\n            -showAttributes 0\n            -showConnected 0\n            -showAnimCurvesOnly 0\n            -showMuteInfo 0\n            -organizeByLayer 1\n"
		+ "            -organizeByClip 1\n            -showAnimLayerWeight 1\n            -autoExpandLayers 1\n            -autoExpand 0\n            -showDagOnly 1\n            -showAssets 1\n            -showContainedOnly 1\n            -showPublishedAsConnected 0\n            -showParentContainers 0\n            -showContainerContents 1\n            -ignoreDagHierarchy 0\n            -expandConnections 0\n            -showUpstreamCurves 1\n            -showUnitlessCurves 1\n            -showCompounds 1\n            -showLeafs 1\n            -showNumericAttrsOnly 0\n            -highlightActive 1\n            -autoSelectNewObjects 0\n            -doNotSelectNewObjects 0\n            -dropIsParent 1\n            -transmitFilters 0\n            -setFilter \"defaultSetFilter\" \n            -showSetMembers 1\n            -allowMultiSelection 1\n            -alwaysToggleSelect 0\n            -directSelect 0\n            -isSet 0\n            -isSetMember 0\n            -displayMode \"DAG\" \n            -expandObjects 0\n            -setsIgnoreFilters 1\n            -containersIgnoreFilters 0\n"
		+ "            -editAttrName 0\n            -showAttrValues 0\n            -highlightSecondary 0\n            -showUVAttrsOnly 0\n            -showTextureNodesOnly 0\n            -attrAlphaOrder \"default\" \n            -animLayerFilterOptions \"allAffecting\" \n            -sortOrder \"none\" \n            -longNames 0\n            -niceNames 1\n            -showNamespace 1\n            -showPinIcons 0\n            -mapMotionTrails 0\n            -ignoreHiddenAttribute 0\n            -ignoreOutlinerColor 0\n            -renderFilterVisible 0\n            -renderFilterIndex 0\n            -selectionOrder \"chronological\" \n            -expandAttribute 0\n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"outlinerPanel\" (localizedPanelLabel(\"Outliner\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\toutlinerPanel -edit -l (localizedPanelLabel(\"Outliner\")) -mbv $menusOkayInPanels  $panelName;\n\t\t$editorName = $panelName;\n        outlinerEditor -e \n"
		+ "            -showShapes 0\n            -showAssignedMaterials 0\n            -showTimeEditor 1\n            -showReferenceNodes 0\n            -showReferenceMembers 0\n            -showAttributes 0\n            -showConnected 0\n            -showAnimCurvesOnly 0\n            -showMuteInfo 0\n            -organizeByLayer 1\n            -organizeByClip 1\n            -showAnimLayerWeight 1\n            -autoExpandLayers 1\n            -autoExpand 0\n            -showDagOnly 1\n            -showAssets 1\n            -showContainedOnly 1\n            -showPublishedAsConnected 0\n            -showParentContainers 0\n            -showContainerContents 1\n            -ignoreDagHierarchy 0\n            -expandConnections 0\n            -showUpstreamCurves 1\n            -showUnitlessCurves 1\n            -showCompounds 1\n            -showLeafs 1\n            -showNumericAttrsOnly 0\n            -highlightActive 1\n            -autoSelectNewObjects 0\n            -doNotSelectNewObjects 0\n            -dropIsParent 1\n            -transmitFilters 0\n"
		+ "            -setFilter \"defaultSetFilter\" \n            -showSetMembers 1\n            -allowMultiSelection 1\n            -alwaysToggleSelect 0\n            -directSelect 0\n            -displayMode \"DAG\" \n            -expandObjects 0\n            -setsIgnoreFilters 1\n            -containersIgnoreFilters 0\n            -editAttrName 0\n            -showAttrValues 0\n            -highlightSecondary 0\n            -showUVAttrsOnly 0\n            -showTextureNodesOnly 0\n            -attrAlphaOrder \"default\" \n            -animLayerFilterOptions \"allAffecting\" \n            -sortOrder \"none\" \n            -longNames 0\n            -niceNames 1\n            -showNamespace 1\n            -showPinIcons 0\n            -mapMotionTrails 0\n            -ignoreHiddenAttribute 0\n            -ignoreOutlinerColor 0\n            -renderFilterVisible 0\n            $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"graphEditor\" (localizedPanelLabel(\"Graph Editor\")) `;\n"
		+ "\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Graph Editor\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAssignedMaterials 0\n                -showTimeEditor 1\n                -showReferenceNodes 0\n                -showReferenceMembers 0\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n                -showMuteInfo 0\n                -organizeByLayer 1\n                -organizeByClip 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 1\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showParentContainers 1\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n"
		+ "                -showUpstreamCurves 1\n                -showUnitlessCurves 1\n                -showCompounds 0\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 1\n                -doNotSelectNewObjects 0\n                -dropIsParent 1\n                -transmitFilters 1\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n"
		+ "                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 1\n                -mapMotionTrails 1\n                -ignoreHiddenAttribute 0\n                -ignoreOutlinerColor 0\n                -renderFilterVisible 0\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"GraphEd\");\n            animCurveEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 1\n                -displayInfinities 0\n                -displayValues 0\n                -autoFit 1\n                -autoFitTime 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -showResults \"off\" \n                -showBufferCurves \"off\" \n                -smoothness \"fine\" \n                -resultSamples 1\n                -resultScreenSamples 0\n                -resultUpdate \"delayed\" \n                -showUpstreamCurves 1\n                -showCurveNames 0\n                -showActiveCurveNames 0\n"
		+ "                -stackedCurves 0\n                -stackedCurvesMin -1\n                -stackedCurvesMax 1\n                -stackedCurvesSpace 0.2\n                -displayNormalized 0\n                -preSelectionHighlight 0\n                -constrainDrag 0\n                -classicMode 1\n                -valueLinesToggle 1\n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dopeSheetPanel\" (localizedPanelLabel(\"Dope Sheet\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Dope Sheet\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"OutlineEd\");\n            outlinerEditor -e \n                -showShapes 1\n                -showAssignedMaterials 0\n                -showTimeEditor 1\n                -showReferenceNodes 0\n                -showReferenceMembers 0\n                -showAttributes 1\n                -showConnected 1\n                -showAnimCurvesOnly 1\n"
		+ "                -showMuteInfo 0\n                -organizeByLayer 1\n                -organizeByClip 1\n                -showAnimLayerWeight 1\n                -autoExpandLayers 1\n                -autoExpand 0\n                -showDagOnly 0\n                -showAssets 1\n                -showContainedOnly 0\n                -showPublishedAsConnected 0\n                -showParentContainers 1\n                -showContainerContents 0\n                -ignoreDagHierarchy 0\n                -expandConnections 1\n                -showUpstreamCurves 1\n                -showUnitlessCurves 0\n                -showCompounds 1\n                -showLeafs 1\n                -showNumericAttrsOnly 1\n                -highlightActive 0\n                -autoSelectNewObjects 0\n                -doNotSelectNewObjects 1\n                -dropIsParent 1\n                -transmitFilters 0\n                -setFilter \"0\" \n                -showSetMembers 0\n                -allowMultiSelection 1\n                -alwaysToggleSelect 0\n                -directSelect 0\n"
		+ "                -displayMode \"DAG\" \n                -expandObjects 0\n                -setsIgnoreFilters 1\n                -containersIgnoreFilters 0\n                -editAttrName 0\n                -showAttrValues 0\n                -highlightSecondary 0\n                -showUVAttrsOnly 0\n                -showTextureNodesOnly 0\n                -attrAlphaOrder \"default\" \n                -animLayerFilterOptions \"allAffecting\" \n                -sortOrder \"none\" \n                -longNames 0\n                -niceNames 1\n                -showNamespace 1\n                -showPinIcons 0\n                -mapMotionTrails 1\n                -ignoreHiddenAttribute 0\n                -ignoreOutlinerColor 0\n                -renderFilterVisible 0\n                $editorName;\n\n\t\t\t$editorName = ($panelName+\"DopeSheetEd\");\n            dopeSheetEditor -e \n                -displayKeys 1\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n"
		+ "                -displayValues 0\n                -autoFit 0\n                -autoFitTime 0\n                -snapTime \"integer\" \n                -snapValue \"none\" \n                -outliner \"dopeSheetPanel1OutlineEd\" \n                -showSummary 1\n                -showScene 0\n                -hierarchyBelow 0\n                -showTicks 1\n                -selectionWindow 0 0 0 0 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"timeEditorPanel\" (localizedPanelLabel(\"Time Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Time Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"clipEditorPanel\" (localizedPanelLabel(\"Trax Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Trax Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\n\t\t\t$editorName = clipEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -displayValues 0\n                -autoFit 0\n                -autoFitTime 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -initialized 0\n                -manageSequencer 0 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"sequenceEditorPanel\" (localizedPanelLabel(\"Camera Sequencer\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Camera Sequencer\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = sequenceEditorNameFromPanel($panelName);\n            clipEditor -e \n                -displayKeys 0\n                -displayTangents 0\n"
		+ "                -displayActiveKeys 0\n                -displayActiveKeyTangents 0\n                -displayInfinities 0\n                -displayValues 0\n                -autoFit 0\n                -autoFitTime 0\n                -snapTime \"none\" \n                -snapValue \"none\" \n                -initialized 0\n                -manageSequencer 1 \n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"hyperGraphPanel\" (localizedPanelLabel(\"Hypergraph Hierarchy\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Hypergraph Hierarchy\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"HyperGraphEd\");\n            hyperGraph -e \n                -graphLayoutStyle \"hierarchicalLayout\" \n                -orientation \"horiz\" \n                -mergeConnections 0\n                -zoom 1\n                -animateTransition 0\n                -showRelationships 1\n"
		+ "                -showShapes 0\n                -showDeformers 0\n                -showExpressions 0\n                -showConstraints 0\n                -showConnectionFromSelected 0\n                -showConnectionToSelected 0\n                -showConstraintLabels 0\n                -showUnderworld 0\n                -showInvisible 0\n                -transitionFrames 1\n                -opaqueContainers 0\n                -freeform 0\n                -imagePosition 0 0 \n                -imageScale 1\n                -imageEnabled 0\n                -graphType \"DAG\" \n                -heatMapDisplay 0\n                -updateSelection 1\n                -updateNodeAdded 1\n                -useDrawOverrideColor 0\n                -limitGraphTraversal -1\n                -range 0 0 \n                -iconSize \"smallIcons\" \n                -showCachedConnections 0\n                $editorName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"hyperShadePanel\" (localizedPanelLabel(\"Hypershade\")) `;\n"
		+ "\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Hypershade\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"visorPanel\" (localizedPanelLabel(\"Visor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Visor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"nodeEditorPanel\" (localizedPanelLabel(\"Node Editor\")) `;\n\tif ($nodeEditorPanelVisible || $nodeEditorWorkspaceControlOpen) {\n\t\tif (\"\" == $panelName) {\n\t\t\tif ($useSceneConfig) {\n\t\t\t\t$panelName = `scriptedPanel -unParent  -type \"nodeEditorPanel\" -l (localizedPanelLabel(\"Node Editor\")) -mbv $menusOkayInPanels `;\n\n\t\t\t$editorName = ($panelName+\"NodeEditorEd\");\n            nodeEditor -e \n                -allAttributes 0\n"
		+ "                -allNodes 0\n                -autoSizeNodes 1\n                -consistentNameSize 1\n                -createNodeCommand \"nodeEdCreateNodeCommand\" \n                -connectNodeOnCreation 0\n                -connectOnDrop 0\n                -copyConnectionsOnPaste 0\n                -connectionStyle \"bezier\" \n                -defaultPinnedState 0\n                -additiveGraphingMode 0\n                -settingsChangedCallback \"nodeEdSyncControls\" \n                -traversalDepthLimit -1\n                -keyPressCommand \"nodeEdKeyPressCommand\" \n                -nodeTitleMode \"name\" \n                -gridSnap 0\n                -gridVisibility 1\n                -crosshairOnEdgeDragging 0\n                -popupMenuScript \"nodeEdBuildPanelMenus\" \n                -showNamespace 1\n                -showShapes 1\n                -showSGShapes 0\n                -showTransforms 1\n                -useAssets 1\n                -syncedSelection 1\n                -extendToShapes 1\n                -editorMode \"default\" \n"
		+ "                -hasWatchpoint 0\n                $editorName;\n\t\t\t}\n\t\t} else {\n\t\t\t$label = `panel -q -label $panelName`;\n\t\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Node Editor\")) -mbv $menusOkayInPanels  $panelName;\n\n\t\t\t$editorName = ($panelName+\"NodeEditorEd\");\n            nodeEditor -e \n                -allAttributes 0\n                -allNodes 0\n                -autoSizeNodes 1\n                -consistentNameSize 1\n                -createNodeCommand \"nodeEdCreateNodeCommand\" \n                -connectNodeOnCreation 0\n                -connectOnDrop 0\n                -copyConnectionsOnPaste 0\n                -connectionStyle \"bezier\" \n                -defaultPinnedState 0\n                -additiveGraphingMode 0\n                -settingsChangedCallback \"nodeEdSyncControls\" \n                -traversalDepthLimit -1\n                -keyPressCommand \"nodeEdKeyPressCommand\" \n                -nodeTitleMode \"name\" \n                -gridSnap 0\n                -gridVisibility 1\n                -crosshairOnEdgeDragging 0\n"
		+ "                -popupMenuScript \"nodeEdBuildPanelMenus\" \n                -showNamespace 1\n                -showShapes 1\n                -showSGShapes 0\n                -showTransforms 1\n                -useAssets 1\n                -syncedSelection 1\n                -extendToShapes 1\n                -editorMode \"default\" \n                -hasWatchpoint 0\n                $editorName;\n\t\t\tif (!$useSceneConfig) {\n\t\t\t\tpanel -e -l $label $panelName;\n\t\t\t}\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"createNodePanel\" (localizedPanelLabel(\"Create Node\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Create Node\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"polyTexturePlacementPanel\" (localizedPanelLabel(\"UV Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"UV Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"renderWindowPanel\" (localizedPanelLabel(\"Render View\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Render View\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"shapePanel\" (localizedPanelLabel(\"Shape Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tshapePanel -edit -l (localizedPanelLabel(\"Shape Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextPanel \"posePanel\" (localizedPanelLabel(\"Pose Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tposePanel -edit -l (localizedPanelLabel(\"Pose Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n"
		+ "\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dynRelEdPanel\" (localizedPanelLabel(\"Dynamic Relationships\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Dynamic Relationships\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"relationshipPanel\" (localizedPanelLabel(\"Relationship Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Relationship Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"referenceEditorPanel\" (localizedPanelLabel(\"Reference Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Reference Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"componentEditorPanel\" (localizedPanelLabel(\"Component Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Component Editor\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"dynPaintScriptedPanelType\" (localizedPanelLabel(\"Paint Effects\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Paint Effects\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"scriptEditorPanel\" (localizedPanelLabel(\"Script Editor\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Script Editor\")) -mbv $menusOkayInPanels  $panelName;\n"
		+ "\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"profilerPanel\" (localizedPanelLabel(\"Profiler Tool\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Profiler Tool\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\t$panelName = `sceneUIReplacement -getNextScriptedPanel \"contentBrowserPanel\" (localizedPanelLabel(\"Content Browser\")) `;\n\tif (\"\" != $panelName) {\n\t\t$label = `panel -q -label $panelName`;\n\t\tscriptedPanel -edit -l (localizedPanelLabel(\"Content Browser\")) -mbv $menusOkayInPanels  $panelName;\n\t\tif (!$useSceneConfig) {\n\t\t\tpanel -e -l $label $panelName;\n\t\t}\n\t}\n\n\n\tif ($useSceneConfig) {\n        string $configName = `getPanel -cwl (localizedPanelLabel(\"Current Layout\"))`;\n        if (\"\" != $configName) {\n\t\t\tpanelConfiguration -edit -label (localizedPanelLabel(\"Current Layout\")) \n\t\t\t\t-userCreated false\n\t\t\t\t-defaultImage \"\"\n"
		+ "\t\t\t\t-image \"\"\n\t\t\t\t-sc false\n\t\t\t\t-configString \"global string $gMainPane; paneLayout -e -cn \\\"single\\\" -ps 1 100 100 $gMainPane;\"\n\t\t\t\t-removeAllPanels\n\t\t\t\t-ap false\n\t\t\t\t\t(localizedPanelLabel(\"Front View\")) \n\t\t\t\t\t\"modelPanel\"\n"
		+ "\t\t\t\t\t\"$panelName = `modelPanel -unParent -l (localizedPanelLabel(\\\"Front View\\\")) -mbv $menusOkayInPanels `;\\n$editorName = $panelName;\\nmodelEditor -e \\n    -cam `findStartUpCamera front` \\n    -useInteractiveMode 0\\n    -displayLights \\\"default\\\" \\n    -displayAppearance \\\"smoothShaded\\\" \\n    -activeOnly 0\\n    -ignorePanZoom 0\\n    -wireframeOnShaded 0\\n    -headsUpDisplay 1\\n    -holdOuts 1\\n    -selectionHiliteDisplay 1\\n    -useDefaultMaterial 0\\n    -bufferMode \\\"double\\\" \\n    -twoSidedLighting 0\\n    -backfaceCulling 0\\n    -xray 0\\n    -jointXray 0\\n    -activeComponentsXray 0\\n    -displayTextures 0\\n    -smoothWireframe 0\\n    -lineWidth 1\\n    -textureAnisotropic 0\\n    -textureHilight 1\\n    -textureSampling 2\\n    -textureDisplay \\\"modulate\\\" \\n    -textureMaxSize 32768\\n    -fogging 0\\n    -fogSource \\\"fragment\\\" \\n    -fogMode \\\"linear\\\" \\n    -fogStart 0\\n    -fogEnd 100\\n    -fogDensity 0.1\\n    -fogColor 0.5 0.5 0.5 1 \\n    -depthOfFieldPreview 1\\n    -maxConstantTransparency 1\\n    -rendererName \\\"vp2Renderer\\\" \\n    -objectFilterShowInHUD 1\\n    -isFiltered 0\\n    -colorResolution 256 256 \\n    -bumpResolution 512 512 \\n    -textureCompression 0\\n    -transparencyAlgorithm \\\"frontAndBackCull\\\" \\n    -transpInShadows 0\\n    -cullingOverride \\\"none\\\" \\n    -lowQualityLighting 0\\n    -maximumNumHardwareLights 1\\n    -occlusionCulling 0\\n    -shadingModel 0\\n    -useBaseRenderer 0\\n    -useReducedRenderer 0\\n    -smallObjectCulling 0\\n    -smallObjectThreshold -1 \\n    -interactiveDisableShadows 0\\n    -interactiveBackFaceCull 0\\n    -sortTransparent 1\\n    -controllers 1\\n    -nurbsCurves 1\\n    -nurbsSurfaces 1\\n    -polymeshes 1\\n    -subdivSurfaces 1\\n    -planes 1\\n    -lights 1\\n    -cameras 1\\n    -controlVertices 1\\n    -hulls 1\\n    -grid 1\\n    -imagePlane 1\\n    -joints 1\\n    -ikHandles 1\\n    -deformers 1\\n    -dynamics 1\\n    -particleInstancers 1\\n    -fluids 1\\n    -hairSystems 1\\n    -follicles 1\\n    -nCloths 1\\n    -nParticles 1\\n    -nRigids 1\\n    -dynamicConstraints 1\\n    -locators 1\\n    -manipulators 1\\n    -pluginShapes 1\\n    -dimensions 1\\n    -handles 1\\n    -pivots 1\\n    -textures 1\\n    -strokes 1\\n    -motionTrails 1\\n    -clipGhosts 1\\n    -greasePencils 1\\n    -shadows 0\\n    -captureSequenceNumber -1\\n    -width 1878\\n    -height 798\\n    -sceneRenderFilter 0\\n    $editorName;\\nmodelEditor -e -viewSelected 0 $editorName\"\n"
		+ "\t\t\t\t\t\"modelPanel -edit -l (localizedPanelLabel(\\\"Front View\\\")) -mbv $menusOkayInPanels  $panelName;\\n$editorName = $panelName;\\nmodelEditor -e \\n    -cam `findStartUpCamera front` \\n    -useInteractiveMode 0\\n    -displayLights \\\"default\\\" \\n    -displayAppearance \\\"smoothShaded\\\" \\n    -activeOnly 0\\n    -ignorePanZoom 0\\n    -wireframeOnShaded 0\\n    -headsUpDisplay 1\\n    -holdOuts 1\\n    -selectionHiliteDisplay 1\\n    -useDefaultMaterial 0\\n    -bufferMode \\\"double\\\" \\n    -twoSidedLighting 0\\n    -backfaceCulling 0\\n    -xray 0\\n    -jointXray 0\\n    -activeComponentsXray 0\\n    -displayTextures 0\\n    -smoothWireframe 0\\n    -lineWidth 1\\n    -textureAnisotropic 0\\n    -textureHilight 1\\n    -textureSampling 2\\n    -textureDisplay \\\"modulate\\\" \\n    -textureMaxSize 32768\\n    -fogging 0\\n    -fogSource \\\"fragment\\\" \\n    -fogMode \\\"linear\\\" \\n    -fogStart 0\\n    -fogEnd 100\\n    -fogDensity 0.1\\n    -fogColor 0.5 0.5 0.5 1 \\n    -depthOfFieldPreview 1\\n    -maxConstantTransparency 1\\n    -rendererName \\\"vp2Renderer\\\" \\n    -objectFilterShowInHUD 1\\n    -isFiltered 0\\n    -colorResolution 256 256 \\n    -bumpResolution 512 512 \\n    -textureCompression 0\\n    -transparencyAlgorithm \\\"frontAndBackCull\\\" \\n    -transpInShadows 0\\n    -cullingOverride \\\"none\\\" \\n    -lowQualityLighting 0\\n    -maximumNumHardwareLights 1\\n    -occlusionCulling 0\\n    -shadingModel 0\\n    -useBaseRenderer 0\\n    -useReducedRenderer 0\\n    -smallObjectCulling 0\\n    -smallObjectThreshold -1 \\n    -interactiveDisableShadows 0\\n    -interactiveBackFaceCull 0\\n    -sortTransparent 1\\n    -controllers 1\\n    -nurbsCurves 1\\n    -nurbsSurfaces 1\\n    -polymeshes 1\\n    -subdivSurfaces 1\\n    -planes 1\\n    -lights 1\\n    -cameras 1\\n    -controlVertices 1\\n    -hulls 1\\n    -grid 1\\n    -imagePlane 1\\n    -joints 1\\n    -ikHandles 1\\n    -deformers 1\\n    -dynamics 1\\n    -particleInstancers 1\\n    -fluids 1\\n    -hairSystems 1\\n    -follicles 1\\n    -nCloths 1\\n    -nParticles 1\\n    -nRigids 1\\n    -dynamicConstraints 1\\n    -locators 1\\n    -manipulators 1\\n    -pluginShapes 1\\n    -dimensions 1\\n    -handles 1\\n    -pivots 1\\n    -textures 1\\n    -strokes 1\\n    -motionTrails 1\\n    -clipGhosts 1\\n    -greasePencils 1\\n    -shadows 0\\n    -captureSequenceNumber -1\\n    -width 1878\\n    -height 798\\n    -sceneRenderFilter 0\\n    $editorName;\\nmodelEditor -e -viewSelected 0 $editorName\"\n"
		+ "\t\t\t\t$configName;\n\n            setNamedPanelLayout (localizedPanelLabel(\"Current Layout\"));\n        }\n\n        panelHistory -e -clear mainPanelHistory;\n        sceneUIReplacement -clear;\n\t}\n\n\ngrid -spacing 1 -size 10 -divisions 10 -displayAxes yes -displayGridLines yes -displayDivisionLines yes -displayPerspectiveLabels no -displayOrthographicLabels no -displayAxesBold yes -perspectiveLabelPosition axis -orthographicLabelPosition edge;\nviewManip -drawCompass 0 -compassAngle 0 -frontParameters \"\" -homeParameters \"\" -selectionLockParameters \"\";\n}\n");
	setAttr ".st" 3;
createNode script -n "sceneConfigurationScriptNode";
	rename -uid "D5DFA9A0-4AE2-79D3-239F-E996683973C1";
	setAttr ".b" -type "string" "playbackOptions -min 1 -max 120 -ast 1 -aet 200 ";
	setAttr ".st" 6;
select -ne :time1;
	setAttr ".o" 1;
	setAttr ".unw" 1;
select -ne :hardwareRenderingGlobals;
	setAttr ".otfna" -type "stringArray" 22 "NURBS Curves" "NURBS Surfaces" "Polygons" "Subdiv Surface" "Particles" "Particle Instance" "Fluids" "Strokes" "Image Planes" "UI" "Lights" "Cameras" "Locators" "Joints" "IK Handles" "Deformers" "Motion Trails" "Components" "Hair Systems" "Follicles" "Misc. UI" "Ornaments"  ;
	setAttr ".otfva" -type "Int32Array" 22 0 1 1 1 1 1
		 1 1 1 0 0 0 0 0 0 0 0 0
		 0 0 0 0 ;
	setAttr ".fprt" yes;
select -ne :renderPartition;
	setAttr -s 2 ".st";
select -ne :renderGlobalsList1;
select -ne :defaultShaderList1;
	setAttr -s 4 ".s";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderingList1;
select -ne :initialShadingGroup;
	setAttr -s 6 ".dsm";
	setAttr ".ro" yes;
	setAttr -s 6 ".gn";
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultResolution;
	setAttr ".pa" 1;
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
select -ne :ikSystem;
	setAttr -s 4 ".sol";
connectAttr "groupParts2.og" "polySurfaceShape4.i";
connectAttr "groupId24.id" "polySurfaceShape4.iog.og[0].gid";
connectAttr ":initialShadingGroup.mwc" "polySurfaceShape4.iog.og[0].gco";
connectAttr "groupParts3.og" "polySurfaceShape5.i";
connectAttr "groupId25.id" "polySurfaceShape5.iog.og[0].gid";
connectAttr ":initialShadingGroup.mwc" "polySurfaceShape5.iog.og[0].gco";
connectAttr "groupParts4.og" "polySurfaceShape6.i";
connectAttr "groupId26.id" "polySurfaceShape6.iog.og[0].gid";
connectAttr ":initialShadingGroup.mwc" "polySurfaceShape6.iog.og[0].gco";
connectAttr "groupParts5.og" "polySurfaceShape7.i";
connectAttr "groupId27.id" "polySurfaceShape7.iog.og[0].gid";
connectAttr ":initialShadingGroup.mwc" "polySurfaceShape7.iog.og[0].gco";
connectAttr "polyChipOff1.out" "polySurface7Shape.i";
connectAttr "groupId23.id" "polySurface7Shape.iog.og[0].gid";
connectAttr ":initialShadingGroup.mwc" "polySurface7Shape.iog.og[0].gco";
connectAttr "polyTweakUV1.uvtk[0]" "polySurface7Shape.uvst[0].uvtw";
connectAttr "groupParts7.og" "polySurface10Shape.i";
connectAttr "groupId29.id" "polySurface10Shape.iog.og[0].gid";
connectAttr ":initialShadingGroup.mwc" "polySurface10Shape.iog.og[0].gco";
connectAttr "polyUnite1.out" "groupParts7.ig";
connectAttr "groupId29.id" "groupParts7.gi";
connectAttr "polySurfaceShape6.o" "polyUnite1.ip[0]";
connectAttr "polySurfaceShape7.o" "polyUnite1.ip[1]";
connectAttr "polySurfaceShape5.o" "polyUnite1.ip[2]";
connectAttr "polySurfaceShape4.o" "polyUnite1.ip[3]";
connectAttr "polySurfaceShape6.wm" "polyUnite1.im[0]";
connectAttr "polySurfaceShape7.wm" "polyUnite1.im[1]";
connectAttr "polySurfaceShape5.wm" "polyUnite1.im[2]";
connectAttr "polySurfaceShape4.wm" "polyUnite1.im[3]";
connectAttr "polySeparate1.out[2]" "groupParts4.ig";
connectAttr "groupId26.id" "groupParts4.gi";
connectAttr "polySurface7Shape.o" "polySeparate1.ip";
connectAttr "polyTweakUV1.out" "polyChipOff1.ip";
connectAttr "polySurface7Shape.wm" "polyChipOff1.mp";
connectAttr "polyPlanarProj1.out" "polyTweakUV1.ip";
connectAttr "polyAutoProj1.out" "polyPlanarProj1.ip";
connectAttr "polySurface7Shape.wm" "polyPlanarProj1.mp";
connectAttr "polyTweak2.out" "polyAutoProj1.ip";
connectAttr "polySurface7Shape.wm" "polyAutoProj1.mp";
connectAttr "polySoftEdge1.out" "polyTweak2.ip";
connectAttr "polyTweak1.out" "polySoftEdge1.ip";
connectAttr "polySurface7Shape.wm" "polySoftEdge1.mp";
connectAttr "polyExtrudeEdge1.out" "polyTweak1.ip";
connectAttr "groupParts1.og" "polyExtrudeEdge1.ip";
connectAttr "polySurface7Shape.wm" "polyExtrudeEdge1.mp";
connectAttr "polySurfaceShape3.o" "groupParts1.ig";
connectAttr "groupId23.id" "groupParts1.gi";
connectAttr "polySeparate1.out[3]" "groupParts5.ig";
connectAttr "groupId27.id" "groupParts5.gi";
connectAttr "polySeparate1.out[1]" "groupParts3.ig";
connectAttr "groupId25.id" "groupParts3.gi";
connectAttr "polySeparate1.out[0]" "groupParts2.ig";
connectAttr "groupId24.id" "groupParts2.gi";
relationship "link" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
connectAttr "layerManager.dli[0]" "defaultLayer.id";
connectAttr "renderLayerManager.rlmi[0]" "defaultRenderLayer.rlid";
connectAttr "defaultRenderLayer.msg" ":defaultRenderingList1.r" -na;
connectAttr "polySurface7Shape.iog.og[0]" ":initialShadingGroup.dsm" -na;
connectAttr "polySurfaceShape4.iog.og[0]" ":initialShadingGroup.dsm" -na;
connectAttr "polySurfaceShape5.iog.og[0]" ":initialShadingGroup.dsm" -na;
connectAttr "polySurfaceShape6.iog.og[0]" ":initialShadingGroup.dsm" -na;
connectAttr "polySurfaceShape7.iog.og[0]" ":initialShadingGroup.dsm" -na;
connectAttr "polySurface10Shape.iog.og[0]" ":initialShadingGroup.dsm" -na;
connectAttr "groupId23.msg" ":initialShadingGroup.gn" -na;
connectAttr "groupId24.msg" ":initialShadingGroup.gn" -na;
connectAttr "groupId25.msg" ":initialShadingGroup.gn" -na;
connectAttr "groupId26.msg" ":initialShadingGroup.gn" -na;
connectAttr "groupId27.msg" ":initialShadingGroup.gn" -na;
connectAttr "groupId29.msg" ":initialShadingGroup.gn" -na;
// End of WheatFlower.ma

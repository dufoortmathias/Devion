<template>
  <div>
    <div>
      <h1>Tekeningen</h1>
    </div>
    <excelFileInput
      :label="file.label"
      :error="file.error"
      :filename="file.filename"
      :showFile="file.showFile"
      @file-updated="handleFileUpdate"
    />
    <Dropdown
      :id="dropdownFolders.id"
      :label="dropdownFolders.label"
      :options="dropdownFolders.options"
      :error="dropdownFolders.error"
      @option-selected="handledropdownFoldersSelected"
      class="c-dropdown"
    />
    <ButtonDevion
      :label="button.label"
      :isDisabled="button.isDisabled"
      :showButton="button.showButton"
      @click="handleButton"
      class="c-button"
    />
    <div v-if="loading.showLoad" class="c-load">
      <LoadingAnimation :showLoad="loading.showLoad" />
    </div>
    <div v-if="fileTable.showTable">
      <div class="c-boek">
        <p>Toggle Errors:</p>
        <BasicToggleSwitch v-model="errorToggle" class="c-toggle" />
      </div>
      <FileTableVue :products="fileTable.products" :errorToggle="errorToggle" />
    </div>
    <div v-if="fileTable.showTable">
      <h2>Boeken</h2>
      <div class="c-boek">
        <p>Maak boeken voor montage:</p>
        <BasicToggleSwitch v-model="LinkModelMontage" class="c-toggle" />
      </div>
      <div class="c-boek">
        <p>Maak boeken voor lassen:</p>
        <BasicToggleSwitch v-model="LinkModelLassen" class="c-toggle" />
      </div>
      <ButtonDevion
        :label="'Maak boeken'"
        :isDisabled="false"
        :showButton="true"
        @click="handleCreateButton"
        class="c-button"
      />
    </div>
    <div v-if="fileTable.showTable">
      <h2>Bewerkingen</h2>
      <div v-for="bewerking in uniqueBewerkingen" :key="bewerking" class="c-boek">
        <p>{{ bewerking }}</p>
        <BasicToggleSwitch v-model="bewerkingenStatus[bewerking]" class="c-toggle" />
      </div>
      <ButtonDevion
        :label="'Kopieer bewerkingen'"
        :isDisabled="false"
        :showButton="true"
        @click="handleKopieerButton"
        class="c-button"
      />
    </div>
    <div v-if="fileTable.showTable">
      <h2>Nabehandelingen</h2>
      <div v-for="Nabehandeling in UniqueNabehandelingen" :key="Nabehandeling" class="c-boek">
        <p>{{ Nabehandeling }}</p>
        <BasicToggleSwitch v-model="nabehandelingenStatus[Nabehandeling]" class="c-toggle" />
      </div>
      <ButtonDevion
        :label="'Kopieer Nabehandelingen'"
        :isDisabled="false"
        :showButton="true"
        @click="handleKopieerButtonNabehandelingen"
        class="c-button"
      />
    </div>
  </div>
</template>
  
  <script>
import ExcelFileInput from '../components/componenten/excelFileInput.vue'
import ButtonDevion from '../components/componenten/ButtonDevion.vue'
import LoadingAnimation from '../components/componenten/LoadingAnimation.vue'
import Dropdown from '../components/componenten/DropdownMenu.vue'
import BasicToggleSwitch from '../components/componenten/ToggleButton.vue'

import { GetData, PostDataWithBody } from '../global/global.js'
import FileTableVue from '../components/componenten/FileTable.vue'

let FileContents = ''
let FileName = ''
let BOMData = []
let project = ''
let MonterenLijst = []
let LasLijst = []
let lijst = []

export default {
  name: 'TekeningenView',
  components: {
    ExcelFileInput,
    ButtonDevion,
    LoadingAnimation,
    Dropdown,
    FileTableVue,
    BasicToggleSwitch,
  },
  data() {
    return {
      LinkModelMontage: false,
      LinkModelLassen: false,
      bewerkingenStatus: {},
      uniqueBewerkingen: [],
      bewerkingenData: [],
      UniqueNabehandelingen: [],
      nabehandelingenStatus: {},
      NabehandelingenData: [],
      errorToggle: false,
      showPopup: false,
      loginDetails: null,
      file: {
        components: {
          ExcelFileInput
        },
        id: 'file',
        label: 'BOM file',
        error: false,
        showLabel: true,
        filename: 'Kies een bestand',
        showFile: true
      },
      button: {
        components: {
          ButtonDevion
        },
        label: 'IMPORTEREN',
        isDisabled: true,
        showButton: true
      },
      loading: {
        components: {
          LoadingAnimation
        },
        showLoad: false
      },
      dropdownFolders: {
        components: {
          Dropdown
        },
        id: 'dropdownFolders',
        label: 'Folder',
        options: [],
        error: false
      },
      fileTable: {
        components: {
          FileTableVue
        },
        products: [],
        showTable: false,
        errorToggle: this.errorToggle
      }
    }
  },
  created() {
    this.getFolders()
  },
  watch: {},
  methods: {
    handleLogin(loginDetails) {
      // Receive the login details from the LoginPopup component
      this.loginDetails = loginDetails
      // You can now use the login details to call your API or perform any other action
      console.log('Login details:', loginDetails)
    },
    handleFileUpdate(file) {
      this.file.error = false
      this.file.showLabel = false
      this.file.showButton = true
      this.file.filename = file.name
      this.file.file = file
      if (project != '') {
        this.button.isDisabled = false
      }
    },
    async handleButton() {
      this.loading.showLoad = true
      let endpoint = 'devion/ets/transformbomexcel'
      FileContents = this.file.file.previewBase64.replace(
        'data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,',
        ''
      )
      FileName = this.file.filename
      var data = {
        FileContents: FileContents
      }
      endpoint += '?fileName=' + FileName
      PostDataWithBody(endpoint, data)
        .then((response) => {
          this.button.isDisabled = true
          BOMData = JSON.parse(response)[0][0]

          endpoint = 'devion/tekeningen/check'
          endpoint += '?project=' + project.replaceAll(' ', '%20')
          PostDataWithBody(endpoint, BOMData)
            .then((response) => {
              BOMData = JSON.parse(response)

              this.loading.showLoad = false
              this.fileTable.products = this.filterItems(JSON.parse(response))
              lijst = this.Items(JSON.parse(response))
              this.quantityOverdracht(JSON.parse(response), null)
              this.fileTable.showTable = true
              this.GetNabehandelingen(this.fileTable.products)
              this.GetDifferentNabehandelingen()
              this.GetBewerkingen(this.fileTable.products)
              this.GetDifferentBewerkingen()
            })
            .catch((error) => {
              this.dropdownFolders.error = true
              console.error(error)
            })
          this.loading.showLoad = false
        })
        .catch((error) => {
          console.error(error.response)
        })
    },
    getFolders() {
      let endpoint = 'devion/foldernames'
      GetData(endpoint)
        .then((response) => {
          let json = response.map((item) => {
            return {
              value: item,
              label: item
            }
          })
          this.dropdownFolders.options = json
        })
        .catch((error) => {
          console.error(error.response)
        })
    },
    async handledropdownFoldersSelected(selectedOption) {
      project = selectedOption

      if (this.file.name != '') {
        this.button.isDisabled = false
      }
    },
    filterItems(item) {
      const filtered = []

      if (item) {
        if (item.parts != []) {
          item.parts.forEach((part) => {
            this.filterItems(part).forEach((filteredItem) => {
              const existingItem = filtered.find((i) => i.number === filteredItem.number)
              if (!existingItem) {
                filtered.push(filteredItem)
              }
            })
          })
        }

        if (
          item.files.dxf != 'N/A' ||
          item.files.pdf != 'N/A' ||
          item.files.stp != 'N/A' ||
          item.files.stl != 'N/A' ||
          item.files.flatDxf != 'N/A'||
          item.files.png != 'N/A'
        ) {
          const existingItem = filtered.find((i) => i.number === item.number)
          if (!existingItem) {
            filtered.push(item)
          }
        }
      }

      return filtered.sort((a, b) => a.number.localeCompare(b.number))
    },
    Items(item) {
      const filtered = []

      if (item) {
        if (item.parts != []) {
          item.parts.forEach((part) => {
            this.filterItems(part).forEach((filteredItem) => {
              filtered.push(filteredItem)
            })
          })
        }

        if (
          item.files.dxf != 'N/A' ||
          item.files.pdf != 'N/A' ||
          item.files.stp != 'N/A' ||
          item.files.stl != 'N/A' ||
          item.files.flatDxf != 'N/A' ||
          item.files.png != 'N/A'
        ) {
          filtered.push(item)
        }
      }

      return filtered.sort((a, b) => a.number.localeCompare(b.number))
    },
    quantityOverdracht(subpart, hoofdpart) {
      if (subpart == null) {
        return
      }
      console.log(hoofdpart, subpart)
      if (hoofdpart != null) {
        subpart.quantity *= hoofdpart.quantity
      }
      if (subpart.parts != []) {
        subpart.parts.forEach((part) => {
          this.quantityOverdracht(part, subpart)
        })
      }
    },
    createMonterenLijst(parts) {
      const index = MonterenLijst.findIndex((i) => i.partNumber === parts.number)
      if (parts.bewerking1.toLowerCase() == 'monteren') {
        if (index !== -1) {
          MonterenLijst[index].Aantal += parts.quantity
        } else {
          MonterenLijst.push({
            partNumber: parts.number,
            Aantal: parts.quantity,
            Done: ''
          })
        }
      }
    },
    createLasLijst(parts) {
      const index = LasLijst.findIndex((i) => i.partNumber === parts.number)
      if (parts.bewerking1.toLowerCase() == 'lassen') {
        if (index !== -1) {
          LasLijst[index].Aantal += parts.quantity
        } else {
          LasLijst.push({
            partNumber: parts.number,
            Aantal: parts.quantity,
            Done: ''
          })
        }
      }
    },
    async handleCreateButton() {
      let boeken = []
      if (this.LinkModelMontage) {
        this.Check(BOMData, 'Monteren')

        let monteerStukken = []
        this.fileTable.products.forEach((item) => {
          if (item.bewerking1.toLowerCase() == 'monteren') {
            monteerStukken.push(item)
          }
        })

        var data = {
          hoofdartikel: this.file.filename.split('_')[0],
          project: project,
          amount: monteerStukken.length
        }

        let endpoint = 'devion/tekeningen/createbook'
        PostDataWithBody(endpoint, data)
          .then(() => {
            MonterenLijst = []
            lijst.forEach((part) => {
              this.createMonterenLijst(part)
            })
            var data = {
              data: MonterenLijst,
              project: project,
              hoofdartikel: this.file.filename.split('_')[0]
            }
            endpoint = 'devion/tekeningen/createmontagelijst'

            PostDataWithBody(endpoint, data).catch((error) => {
              console.error(error)
            })
          })
          .catch((error) => {
            console.error(error)
          })
      }
      if (this.LinkModelLassen) {
        boeken.push('Lassen')

        this.Check(BOMData, 'Lassen')
        LasLijst = []
        lijst.forEach((part) => {
          this.createLasLijst(part)
        })
        data = {
          data: LasLijst,
          project: project,
          hoofdartikel: this.file.filename.split('_')[0]
        }
        let endpoint = 'devion/tekeningen/createlaslijst'
        PostDataWithBody(endpoint, data).catch((error) => {
          console.error(error)
        })
      }
      alert('Boeken gemaakt')
    },
    async Check(Part, Bewerking) {
      if (Bewerking == 'Monteren') {
        if (Part != null) {
          let data = {
            MainPart: Part,
            Project: project,
            Boeken: ['Monteren'],
            hoofdartikel: this.file.filename.split('_')[0]
          }
          let endpoint = 'devion/tekeningen/createbooks'
          if (this.LinkModelMontage) {
            PostDataWithBody(endpoint, data).catch((error) => {
              console.error(error)
            })
          }
          Part.parts.forEach((part) => {
            if (part != null) {
              if (part.bewerking1.toLowerCase() == 'monteren') {
                this.Check(part, 'Monteren')
              } else if (part.bewerking1.toLowerCase() == 'lassen') {
                this.Check(part, 'Lassen')
              }
            }
          })
        }
      } else if (Bewerking == 'Lassen') {
        if (Part != null) {
          if (Part.parts != []) {
            if (Part.bewerking1.toLowerCase() == 'lassen') {
              let data = {
                MainPart: Part,
                Project: project,
                Boeken: ['Lassen'],
                hoofdartikel: this.file.filename.split('_')[0]
              }
              let endpoint = 'devion/tekeningen/createbooks'
              if (this.LinkModelLassen) {
                PostDataWithBody(endpoint, data).catch((error) => {
                  console.error(error)
                })
              }
            }
            Part.parts.forEach((part) => {
              if (part != null) {
                if (part.bewerking1.toLowerCase() == 'monteren') {
                  this.Check(part, 'Monteren')
                } else if (part.bewerking1.toLowerCase() == 'lassen') {
                  this.Check(part, 'Lassen')
                }
              }
            })
          }
        }
      }
    },
    GetBewerkingen(artikels) {
      artikels.forEach((artikel) => {
        if (artikel.bewerking1 != null && artikel.bewerking1 != '' && artikel.bewerking1 != '-') {
          let data = {
            bewerking: artikel.bewerking1.toLowerCase(),
            artikel: artikel.number,
            leverancier: artikel.mainSupplier
          }
          this.bewerkingenData.push(data)
        }
        if (artikel.bewerking2 != null && artikel.bewerking2 != '' && artikel.bewerking2 != '-') {
          let data = {
            bewerking: artikel.bewerking2.toLowerCase(),
            artikel: artikel.number,
            leverancier: artikel.mainSupplier
          }
          this.bewerkingenData.push(data)
        }
        if (artikel.bewerking3 != null && artikel.bewerking3 != '' && artikel.bewerking3 != '-') {
          let data = {
            bewerking: artikel.bewerking3.toLowerCase(),
            artikel: artikel.number,
            leverancier: artikel.mainSupplier
          }
          this.bewerkingenData.push(data)
        }
        if (artikel.bewerking4 != null && artikel.bewerking4 != '' && artikel.bewerking4 != '-') {
          let data = {
            bewerking: artikel.bewerking4.toLowerCase(),
            artikel: artikel.number,
            leverancier: artikel.mainSupplier
          }
          this.bewerkingenData.push(data)
        }
      })
    },
    GetDifferentNabehandelingen() {
      this.UniqueNabehandelingen = [
        ...new Set(this.NabehandelingenData.map((item) => item.nabehandeling))
      ]

      this.UniqueNabehandelingen.forEach((nabehandeling) => {
        const index = this.NabehandelingenData.findIndex(
          (item) => item.nabehandeling === nabehandeling
        )
        if (index !== -1) {
          this.NabehandelingenData.splice(index, 1, false)
        }
      })
    },
    GetNabehandelingen(artikels) {
      artikels.forEach((artikel) => {
        if (
          artikel.nabehandeling1 != null &&
          artikel.nabehandeling1 != '' &&
          artikel.nabehandeling1 != '-'
        ) {
          let data = {
            nabehandeling: artikel.nabehandeling1.toLowerCase(),
            artikel: artikel.number
          }

          this.NabehandelingenData.push(data)
        }

        if (
          artikel.nabehandeling2 != null &&
          artikel.nabehandeling2 != '' &&
          artikel.nabehandeling2 != '-'
        ) {
          let data = {
            nabehandeling: artikel.nabehandeling2.toLowerCase(),
            artikel: artikel.number
          }
          this.NabehandelingenData.push(data)
        }
      })
    },
    GetDifferentBewerkingen() {
      this.uniqueBewerkingen = [...new Set(this.bewerkingenData.map((item) => item.bewerking))]

      this.uniqueBewerkingen.forEach((bewerking) => {
        const index = this.bewerkingenData.findIndex((item) => item.bewerking === bewerking)
        if (index !== -1) {
          this.bewerkingenData.splice(index, 1, false)
        }
      })
    },
    handleKopieerButton() {
      let bewerkingen = []
      this.bewerkingenData.forEach((bewerking) => {
        if (this.bewerkingenStatus[bewerking.bewerking]) {
          // add to a list if bewerking is not already in the list
          if (!bewerkingen.includes(bewerking.bewerking)) {
            bewerkingen.push(bewerking.bewerking)
          }
        }
      })

      var data = {
        Folders: bewerkingen,
        BasePath: project
      }
      let endpoint = 'devion/tekeningen/bewerkingen/delete'
      PostDataWithBody(endpoint, data)
      this.bewerkingenData.forEach((bewerking) => {
        if (this.bewerkingenStatus[bewerking.bewerking]) {
          var data = {
            bewerking: bewerking.bewerking,
            artikel: bewerking.artikel,
            leverancier: bewerking.leverancier,
            project: project
          }

          let endpoint = 'devion/tekeningen/kopieer/bewerkingen'
          PostDataWithBody(endpoint, data).catch((error) => {
            console.error(error)
          })
        }
      })
      alert('Kopieer actie voltooid')
    },
    handleKopieerButtonNabehandelingen() {
      let nabehandelingen = []
      this.NabehandelingenData.forEach((nabehandeling) => {
        if (this.nabehandelingenStatus[nabehandeling.nabehandeling]) {
          // add to a list if nabehandeling is not already in the list
          if (!nabehandelingen.includes(nabehandeling.nabehandeling)) {
            nabehandelingen.push(nabehandeling.nabehandeling)
          }
        }
      })

      var data = {
        Folders: nabehandelingen,
        BasePath: project
      }
      let endpoint = 'devion/tekeningen/nabehandelingen/delete'
      PostDataWithBody(endpoint, data)
      let dataset = []
      this.NabehandelingenData.forEach((nabehandeling) => {
        if (this.nabehandelingenStatus[nabehandeling.nabehandeling]) {
          var data = {
            nabehandeling: nabehandeling.nabehandeling,
            artikel: nabehandeling.artikel,
            project: project,
            hoofdArtikel: this.file.filename.split('_')[0]
          }
          dataset.push(data)
        }
      })
      endpoint = 'devion/tekeningen/kopieer/nabehandelingen'
      PostDataWithBody(endpoint, dataset).catch((error) => {
        console.error(error)
      })
      this.showModalNabehandelingen = true
      alert('Kopieer actie voltooid')
    }
  }
}
</script>
  
  <style lang="css">
.c-load {
  margin-top: 2rem;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
}

.c-button {
  margin-top: var(--global-whitespace-lg);
  margin-bottom: var(--global-whitespace-lg);
  cursor: pointer;
}

.c-boek {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 30vw;
}
</style>
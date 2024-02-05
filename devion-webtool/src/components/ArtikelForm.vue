<template>
    <div class="c-artikel_form" v-show=showform>
        <textInput :id="artikelNr.id" :label="artikelNr.label" :error="artikelNr.error" :placeholder="artikelNr.placeholder"
            :errorText="artikelNr.errorText" class="c-artikelNr" @option-selected="handleArtikelNr" />
        <textInput :id="reflev.id" :label="reflev.label" :error="reflev.error" :placeholder="reflev.placeholder"
            :errorText="reflev.errorText" class="c-reflev" @option-selected="handleReflev" />
        <textInput :id="omschrijving.id" :label="omschrijving.label" :error="omschrijving.error"
            :placeholder="omschrijving.placeholder" :error-text="omschrijving.errorText" class="c-omschrijving"
            @option-selected="handleOmschrijving" />
        <textInput :id="tarief.id" :label="tarief.label" :error="tarief.error" :placeholder="tarief.placeholder"
            :errorText="tarief.errorText" class="c-tarief" @option-selected="handletarief" />
        <textInput :id="aankoop.id" :label="aankoop.label" :error="aankoop.error" :placeholder="aankoop.placeholder"
            :errorText="aankoop.errorText" class="c-aankoop" @option-selected="handleAankoop" />
        <textInput :id="stdKorting.id" :label="stdKorting.label" :error="stdKorting.error" :errorText="stdKorting.errorText"
            :placeholder="stdKorting.placeholder" class="c-stdKorting" @option-selected="handleStdKorting" />
        <dropdownMenu :id="muntcode.id" :label="muntcode.label" :options="muntcode.options" :error="muntcode.error"
            class="c-muntcode" :selected="muntcode.selected" @option-selected="handleMuntcode" />
        <textInput :id="verkoop.id" :label="verkoop.label" :error="verkoop.error" :placeholder="verkoop.placeholder"
            :errorText="verkoop.errorText" class="c-verkoop" @option-selected="handleVerkoop" />
        <textInput :id="winstpercentage.id" :label="winstpercentage.label" :error="winstpercentage.error"
            :errorText="winstpercentage.errorText" :placeholder="winstpercentage.placeholder" class="c-winstpercentage"
            @option-selected="handleWinst" />
        <dropdownMenu :id="rekver.id" :label="rekver.label" :options="rekver.options" :error="rekver.error" class="c-rekver"
            :selected="rekver.selected" @option-selected="handleRekver" />
        <dropdownMenu :id="aaneh.id" :label="aaneh.label" :options="aaneh.options" :error="aaneh.error"
            :selected="aaneh.selected" class="c-aaneh" @option-selected="handleAaneh" ref="aaneh" />
        <dropdownMenu :id="vereh.id" :label="vereh.label" :options="vereh.options" :error="vereh.error"
            :selected="vereh.selected" class="c-vereh" @option-selected="handleVereh" ref="vereh" />
        <dropdownMenu :id="btwcode.id" :label="btwcode.label" :options="btwcode.options" :error="btwcode.error"
            class="c-btwcode" :selected="btwcode.selected" @option-selected="handleBtwcode" />
        <textInput :id="omrekfac.id" :label="omrekfac.label" :error="omrekfac.error" :placeholder="omrekfac.placeholder"
            :errorText="omrekfac.errorText" class="c-omrekfac" @option-selected="handleOmrekfac" />
        <dropdownMenu :id="typfac.id" :label="typfac.label" :options="typfac.options" :error="typfac.error" class="c-typfac"
            :selected="typfac.selected" @option-selected="handleTypfac" />
        <textInput :id="merk.id" :label="merk.label" :error="merk.error" :placeholder="merk.placeholder" class="c-merk"
            :errorText="merk.errorText" @option-selected="handleMerk" />
        <dropdownMenu :id="familie.id" :label="familie.label" :options="familie.options" :error="familie.error"
            class="c-familie" :selected="familie.selected" @option-selected="handleFamilie" />
        <dropdownMenu :id="subfamilie.id" :label="subfamilie.label" :options="subfamilie.options" :error="subfamilie.error"
            class="c-subfamilie" :selected="subfamilie.selected" @option-selected="handleSubfamilie" />
        <textInput :id="lengte.id" :label="lengte.label" :error="lengte.error" :placeholder="lengte.placeholder"
            :errorText="lengte.errorText" class="c-lengte" @option-selected="handleLengte" />
        <textInput :id="breedte.id" :label="breedte.label" :error="breedte.error" :placeholder="breedte.placeholder"
            :errorText="breedte.errorText" class="c-breedte" @option-selected="handleBreedte" />
        <textInput :id="hoogte.id" :label="hoogte.label" :error="hoogte.error" :placeholder="hoogte.placeholder"
            :errorText="hoogte.errorText" class="c-hoogte" @option-selected="handleHoogte" />
        <textInput :id="hoofdleverancier.id" :label="hoofdleverancier.label" :options="hoofdleverancier.options"
            :error="hoofdleverancier.error" class="c-hoofdleverancier" :selected="hoofdleverancier.selected"
            @option-selected="handleHoofdleverancier" :placeholder="hoofdleverancier.placeholder" />
        <textInput :id="minaan.id" :label="minaan.label" :error="minaan.error" :placeholder="minaan.placeholder"
            :errorText="minaan.errorText" class="c-minaan" @option-selected="handleMinaan" />
    </div>
</template>

<script>
import textInput from '../components/componenten/textInput.vue'
import dropdownMenu from '../components/componenten/DropdownMenu.vue'
import { GetData } from '../global/global'

const companie = 'devion'
let artikelNr, reflev, omschrijving, tarief, aankoop, stdKorting, muntcode, verkoop, winstpercentage, rekver, aaneh, vereh, btwcode, omrekfac, typfac, merk, familie, subfamilie, lengte, breedte, hoogte, hoofdleverancier, minaan

let object = {}

export default {
    props: {
        showform: Boolean,
        data: Object,
        mass: Float64Array
    },
    components: {
        textInput,
        dropdownMenu,
    },
    data() {
        return {
            artikelNr: {
                components: {
                    textInput,
                },
                id: 'artikelNr',
                label: 'Artikel nummer',
                error: false,
                placeholder: 'artikel nummer',
                errorText: 'Artikel nummer is verplicht'
            },
            reflev: {
                components: {
                    textInput,
                },
                id: 'reflev',
                label: 'Referentie leverancier',
                error: false,
                placeholder: 'referentie leverancier',
                errorText: 'Referentie leverancier is verplicht'
            },
            omschrijving: {
                components: {
                    textInput,
                },
                id: 'omschrijving',
                label: 'Omschrijving',
                error: false,
                placeholder: 'omschrijving',
                errorText: 'Omschrijving is verplicht'
            },
            tarief: {
                components: {
                    textInput,
                },
                id: 'tarief',
                label: 'Catalogus prijs',
                error: false,
                placeholder: 'catalogus prijs',
                errorText: 'Catalogus prijs is verplicht'
            },
            aankoop: {
                components: {
                    textInput,
                },
                id: 'aankoop',
                label: 'Aankoop prijs',
                error: false,
                placeholder: 'aankoop prijs',
                errorText: 'Aankoop prijs is verplicht'
            },
            stdKorting: {
                components: {
                    textInput,
                },
                id: 'stdKorting',
                label: 'Standaard korting (%)',
                error: false,
                placeholder: 'standaard korting',
                errorText: 'Standaard korting is verplicht'
            },
            muntcode: {
                components: {
                    dropdownMenu,
                },
                id: 'muntcode',
                label: 'Muntcode',
                options: [],
                error: false,
            },
            verkoop: {
                components: {
                    textInput,
                },
                id: 'verkoop',
                label: 'Verkoop prijs',
                error: false,
                placeholder: 'verkoop prijs',
                errorText: 'Verkoop prijs is verplicht'
            },
            winstpercentage: {
                components: {
                    textInput,
                },
                id: 'winstpercentage',
                label: 'Winst percentage (%)',
                error: false,
                placeholder: 'winst percentage',
                errorText: 'Winst percentage is verplicht'
            },
            rekver: {
                components: {
                    dropdownMenu,
                },
                id: 'rekver',
                label: 'Rekening verkoop',
                options: [],
                error: false,
            },
            aaneh: {
                components: {
                    dropdownMenu,
                },
                id: 'aaneh',
                label: 'Aankoop eenheid',
                options: [],
                error: false,
            },
            vereh: {
                components: {
                    dropdownMenu,
                },
                id: 'vereh',
                label: 'Verkoop eenheid',
                options: [],
                error: false,
            },
            btwcode: {
                components: {
                    dropdownMenu,
                },
                id: 'btwcode',
                label: 'BTW code',
                options: [],
                error: false,
            },
            omrekfac: {
                components: {
                    textInput,
                },
                id: 'omrekfac',
                label: 'Omrekeningsfactor',
                error: false,
                placeholder: '1',
                errorText: 'Omrekeningsfactor is verplicht'
            },
            typfac: {
                components: {
                    dropdownMenu,
                },
                id: 'typfac',
                label: 'Type factor',
                options: [{ label: 'Deelfactor', value: '0' }, { label: 'Vermenigvuldigingsfactor', value: '1' }],
                error: false,
                selected: '1',
            },
            merk: {
                components: {
                    textInput,
                },
                id: 'merk',
                label: 'Merk',
                error: false,
                placeholder: 'merk',
                errorText: 'Merk is verplicht'
            },
            familie: {
                components: {
                    dropdownMenu,
                },
                id: 'familie',
                label: 'Familie',
                options: [],
                error: false,
            },
            subfamilie: {
                components: {
                    dropdownMenu,
                },
                id: 'subfamilie',
                label: 'Subfamilie',
                options: [],
                error: false,
            },
            lengte: {
                components: {
                    textInput,
                },
                id: 'lengte',
                label: 'Lengte',
                error: false,
                placeholder: '0',
                errorText: 'Lengte is verplicht'
            },
            breedte: {
                components: {
                    textInput,
                },
                id: 'breedte',
                label: 'Breedte',
                error: false,
                placeholder: '0',
                errorText: 'Breedte is verplicht'
            },
            hoogte: {
                components: {
                    textInput,
                },
                id: 'hoogte',
                label: 'Hoogte',
                error: false,
                placeholder: '0',
                errorText: 'Hoogte is verplicht'
            },
            hoofdleverancier: {
                components: {
                    textInput,
                },
                id: 'hoofdleverancier',
                label: 'Hoofdleverancier',
                error: false,
                errorText: 'Hoofdleverancier is verplicht',
                placeholder: 'hoofdleverancier',
            },
            minaan: {
                components: {
                    textInput,
                },
                id: 'minaan',
                label: 'Minimum aantal',
                error: false,
                placeholder: '1',
                errorText: 'Minimum aantal is verplicht'
            },
        }
    },
    created() {
        this.getOptions()
    },
    watch: {
        data(object) {
            console.log(object)
            if (object.number == undefined) {
                this.artikelNr.placeholder = object.artikelNr
                artikelNr = object.artikelNr
                this.reflev.placeholder = object.reflev
                reflev = object.reflev
                this.omschrijving.placeholder = object.omschrijving
                omschrijving = object.omschrijving
                this.tarief.placeholder = object.tarief
                tarief = object.tarief
                this.aankoop.placeholder = object.aankoop
                aankoop = object.aankoop
                this.stdKorting.placeholder = object.stdKorting
                stdKorting = object.stdKorting
                this.verkoop.placeholder = object.verkoop
                verkoop = object.verkoop
                this.winstpercentage.placeholder = object.winstpercentage
                winstpercentage = object.winstpercentage
                this.muntcode.selected = object.muntcode
                muntcode = object.muntcode
                this.rekver.selected = object.rekver
                rekver = object.rekver
                this.aaneh.selected = object.aaneh
                aaneh = object.aaneh
                this.vereh.selected = object.vereh
                vereh = object.vereh
                this.btwcode.selected = object.btwcode
                btwcode = object.btwcode
                this.omrekfac.placeholder = object.omrekfac
                omrekfac = object.omrekfac
                this.typfac.selected = object.typfac
                typfac = object.typfac
                this.merk.placeholder = object.merk
                merk = object.merk
                this.familie.selected = object.familie
                familie = object.familie
                this.subfamilie.selected = object.subfamilie
                subfamilie = object.subfamilie
                this.lengte.placeholder = object.lengte
                lengte = object.lengte
                this.breedte.placeholder = object.breedte
                breedte = object.breedte
                this.hoogte.placeholder = object.hoogte
                hoogte = object.hoogte
                this.hoofdleverancier.selected = object.mainSupplier
                hoofdleverancier = object.mainSupplier
                this.minaan.placeholder = object.minaan
                minaan = object.minaan
            } else if (object.existsMet == false){
                console.log(object)
                this.returnToDefault()

                this.artikelNr.placeholder = object.number
                artikelNr = object.number
                this.reflev.placeholder = object.number
                reflev = object.number
                if (object.omschrijving != "") {
                    this.omschrijving.placeholder = object.description
                    omschrijving = object.description
                }
                console.log(object.bewerking1)
                this.hoofdleverancier.placeholder = object.bewerking1.toString()
                hoofdleverancier = object.bewerking1.toString()
                this.merk.placeholder = "devion"
                merk = "devion"
                this.familie.selected = this.familie.options.find((x) => x.label.toLowerCase() == 'niet courant materiaal').value
                familie = this.familie.options.find((x) => x.label.toLowerCase() == 'niet courant materiaal').value
                this.subfamilie.selected = this.subfamilie.options.find((x) => x.label.toLowerCase() == "non stock").value
                subfamilie = this.subfamilie.options.find((x) => x.label.toLowerCase() == "non stock").value
                this.lengte.placeholder = "0"
                lengte = "0"
                this.breedte.placeholder = "0"
                breedte = "0"
                this.hoogte.placeholder = "0"
                hoogte = "0"
                this.omrekfac.placeholder = "1"
                omrekfac = "1"
                this.typfac.selected = this.typfac.options.find((x) => x.label.toLowerCase() == "vermenigvuldigingsfactor").value
                typfac = this.typfac.options.find((x) => x.label.toLowerCase() == "vermenigvuldigingsfactor").value
                this.muntcode.selected = this.muntcode.options.find((x) => x.label.toLowerCase() == "euro").value
                muntcode = this.muntcode.options.find((x) => x.label.toLowerCase() == "euro").value
                this.rekver.selected = this.rekver.options.find((x) => x.value == "700000").value
                rekver = this.rekver.options.find((x) => x.value == "700000").value
                this.tarief.placeholder = "0"
                tarief = "0"
                this.aankoop.placeholder = "0"
                aankoop = "0"
                this.stdKorting.placeholder = "0"
                stdKorting = "0"
                this.verkoop.placeholder = (aankoop * (1 + (1 / 3))).toFixed(2).toString()
                verkoop = (aankoop * (1 + (1 / 3))).toFixed(2)
                console.log(aankoop, verkoop, object.mass, this.mass, (1 + (1 / 3)))
                this.winstpercentage.placeholder = "33.33"
                winstpercentage = "33.33"
                this.minaan.placeholder = "1"
                minaan = "1"
            }else if (object.existsDev == false){
                console.log(object)
                this.returnToDefault()

                this.artikelNr.placeholder = object.number
                artikelNr = object.number
                this.reflev.placeholder = object.number
                reflev = object.number
                if (object.omschrijving != "") {
                    this.omschrijving.placeholder = object.description
                    omschrijving = object.description
                }
                this.hoofdleverancier.placeholder = object.bewerking1
                hoofdleverancier = object.bewerking1
                this.merk.placeholder = "devion"
                merk = "devion"
                this.familie.selected = this.familie.options.find((x) => x.label.toLowerCase() == 'niet courant materiaal').value
                familie = this.familie.options.find((x) => x.label.toLowerCase() == 'niet courant materiaal').value
                this.subfamilie.selected = this.subfamilie.options.find((x) => x.label.toLowerCase() == "non stock").value
                subfamilie = this.subfamilie.options.find((x) => x.label.toLowerCase() == "non stock").value
                this.lengte.placeholder = "0"
                lengte = "0"
                this.breedte.placeholder = "0"
                breedte = "0"
                this.hoogte.placeholder = "0"
                hoogte = "0"
                this.omrekfac.placeholder = "1"
                omrekfac = "1"
                this.typfac.selected = this.typfac.options.find((x) => x.label.toLowerCase() == "vermenigvuldigingsfactor").value
                typfac = this.typfac.options.find((x) => x.label.toLowerCase() == "vermenigvuldigingsfactor").value
                this.muntcode.selected = this.muntcode.options.find((x) => x.label.toLowerCase() == "euro").value
                muntcode = this.muntcode.options.find((x) => x.label.toLowerCase() == "euro").value
                this.rekver.selected = this.rekver.options.find((x) => x.value == "700000").value
                rekver = this.rekver.options.find((x) => x.value == "700000").value
                this.tarief.placeholder = "0"
                tarief = "0"

                // check if number ends with a W
                if (object.number.charAt(object.number.length - 1) == 'W')
                {
                    
                    this.aankoop.placeholder = (object.mass * this.mass).toString()
                    aankoop = object.mass * this.mass
                } else {
                    this.aankoop.placeholder = "0"
                    aankoop = 0
                }
                this.stdKorting.placeholder = "0"
                stdKorting = "0"
                this.verkoop.placeholder = (aankoop * (1 + (1 / 3))).toString()
                verkoop = aankoop * (1 + (1 / 3))
                this.winstpercentage.placeholder = "33.33"
                winstpercentage = "33.33"
                this.minaan.placeholder = "1"
                minaan = "1"
            }
            else if (object.number != undefined) {
                console.log(object)
                this.returnToDefault()

                this.artikelNr.placeholder = object.number
                artikelNr = object.number
                this.reflev.placeholder = object.number
                reflev = object.number
                if (object.omschrijving != "") {
                    this.omschrijving.placeholder = object.description
                    omschrijving = object.description
                }
                if (object.bewerking1 != '') {
                    this.hoofdleverancier.placeholder = object.bewerking1
                    hoofdleverancier = object.bewerking1
                } else {
                    this.hoofdleverancier.selected = object.mainSupplier
                    hoofdleverancier = object.mainSupplier
                }
                this.merk.placeholder = "devion"
                merk = "devion"
                this.familie.selected = this.familie.options.find((x) => x.label.toLowerCase() == 'niet courant materiaal').value
                familie = this.familie.options.find((x) => x.label.toLowerCase() == 'niet courant materiaal').value
                this.subfamilie.selected = this.subfamilie.options.find((x) => x.label.toLowerCase() == "non stock").value
                subfamilie = this.subfamilie.options.find((x) => x.label.toLowerCase() == "non stock").value
                this.lengte.placeholder = "0"
                lengte = "0"
                this.breedte.placeholder = "0"
                breedte = "0"
                this.hoogte.placeholder = "0"
                hoogte = "0"
                this.omrekfac.placeholder = "1"
                omrekfac = "1"
                this.typfac.selected = this.typfac.options.find((x) => x.label.toLowerCase() == "vermenigvuldigingsfactor").value
                typfac = this.typfac.options.find((x) => x.label.toLowerCase() == "vermenigvuldigingsfactor").value
                this.muntcode.selected = this.muntcode.options.find((x) => x.label.toLowerCase() == "euro").value
                muntcode = this.muntcode.options.find((x) => x.label.toLowerCase() == "euro").value
                this.rekver.selected = this.rekver.options.find((x) => x.value == "700000").value
                rekver = this.rekver.options.find((x) => x.value == "700000").value
                this.tarief.placeholder = "0"
                tarief = "0"
                this.aankoop.placeholder = "0"
                aankoop = "0"
                this.stdKorting.placeholder = "0"
                stdKorting = "0"
                this.verkoop.placeholder = "0"
                verkoop = "0"
                this.winstpercentage.placeholder = "0"
                winstpercentage = "0"
                this.minaan.placeholder = "1"
                minaan = "1"

            } else {
                this.returnToDefault()

                this.artikelNr.placeholder = object.reflev
                artikelNr = object.reflev
                this.reflev.placeholder = object.reflev
                reflev = object.reflev
                this.omschrijving.placeholder = object.omschrijving
                omschrijving = object.omschrijving
                this.tarief.placeholder = object.tarief.toString()
                tarief = object.tarief.toString()
                this.aankoop.placeholder = object.aankoop.toString()
                aankoop = object.aankoop.toString()
                this.stdKorting.placeholder = (parseFloat((1 - (object.aankoop / object.tarief)) * 100).toFixed(4)).toString()
                stdKorting = (parseFloat((1 - (object.aankoop / object.tarief)) * 100).toFixed(4)).toString()
                this.verkoop.placeholder = (parseFloat((object.aankoop * (1 + (1 / 3)))).toFixed(4)).toString()
                verkoop = (parseFloat((object.aankoop * (1 + (1 / 3)))).toFixed(4)).toString()
                this.winstpercentage.placeholder = (parseFloat((1 / 3) * 100).toFixed(4)).toString()
                winstpercentage = (parseFloat((1 / 3) * 100).toFixed(4)).toString()
                this.merk.placeholder = object.merk
                merk = object.merk
                this.minaan.placeholder = object.minaan.toString()
                minaan = object.minaan.toString()
                if (object.bewerking1 != '') {
                    this.hoofdleverancier.placeholder = object.bewerking1
                    hoofdleverancier = object.bewerking1
                } else {
                    this.hoofdleverancier.selected = object.mainSupplier
                    hoofdleverancier = object.mainSupplier
                }
            }
        },
    },
    beforeUnmount() {
        this.returnToDefault()
        // set all variables to null
        artikelNr = null
        reflev = null
        omschrijving = null
        tarief = null
        aankoop = null
        stdKorting = null
        muntcode = null
        verkoop = null
        winstpercentage = null
        rekver = null
        aaneh = null
        vereh = null
        btwcode = null
        omrekfac = null
        typfac = null
        merk = null
        familie = null
        subfamilie = null
        lengte = null
        breedte = null
        hoogte = null
        hoofdleverancier = null
        minaan = null

        object = {}

    },
    methods: {
        emitObjectArtikel() {
            // Emit the selected option when the value changes
            this.$emit('object-artikel', object)
        },
        returnToDefault() {
            this.artikelNr.error = false
            this.reflev.error = false
            this.omschrijving.error = false
            this.tarief.error = false
            this.aankoop.error = false
            this.stdKorting.error = false
            this.muntcode.error = false
            this.verkoop.error = false
            this.winstpercentage.error = false
            this.rekver.error = false
            this.aaneh.error = false
            this.vereh.error = false
            this.btwcode.error = false
            this.omrekfac.error = false
            this.typfac.error = false
            this.merk.error = false
            this.familie.error = false
            this.subfamilie.error = false
            this.lengte.error = false
            this.breedte.error = false
            this.hoogte.error = false
            this.hoofdleverancier.error = false
            this.minaan.error = false

            this.muntcode.selected = this.muntcode.options.find((x) => x.label.toLowerCase() == "euro").value
            muntcode = this.muntcode.options.find((x) => x.label.toLowerCase() == "euro").value
            this.rekver.selected = this.rekver.options.find((x) => x.value == "700000").value
            rekver = this.rekver.options.find((x) => x.value == "700000").value
            this.btwcode.selected = this.btwcode.options.find((x) => x.label.toLowerCase() == 'btw 21 %').value
            btwcode = this.btwcode.options.find((x) => x.label.toLowerCase() == 'btw 21 %').value
            this.familie.selected = this.familie.options.find((x) => x.label.toLowerCase() == 'niet courant materiaal').value
            familie = this.familie.options.find((x) => x.label.toLowerCase() == 'niet courant materiaal').value
            this.subfamilie.selected = this.subfamilie.options.find((x) => x.label.toLowerCase() == "non stock").value
            subfamilie = this.subfamilie.options.find((x) => x.label.toLowerCase() == "non stock").value
            this.typfac.selected = this.typfac.options.find((x) => x.label.toLowerCase() == "vermenigvuldigingsfactor").value
            typfac = this.typfac.options.find((x) => x.label.toLowerCase() == "vermenigvuldigingsfactor").value
            this.hoofdleverancier.selected = ""
            hoofdleverancier = ""
            this.lengte.placeholder = "0"
            lengte = "0"
            this.breedte.placeholder = "0"
            breedte = "0"
            this.hoogte.placeholder = "0"
            hoogte = "0"
            this.omrekfac.placeholder = "1"
            omrekfac = "1"
            this.typfac.selected = this.typfac.options.find((x) => x.label.toLowerCase() == "vermenigvuldigingsfactor").value
            typfac = this.typfac.options.find((x) => x.label.toLowerCase() == "vermenigvuldigingsfactor").value
            this.$refs.aaneh.reset();
            aaneh = undefined
            this.$refs.vereh.reset();
            vereh = undefined
            this.omschrijving.placeholder = "omschrijving"
            omschrijving = undefined
            this.aankoop.placeholder = "0"
            aankoop = 0
            this.winstpercentage.placeholder = "33.33%"
            winstpercentage = 33.33
            this.verkoop.placeholder = parseFloat(aankoop * (1 + 1 / 3)).toFixed(4)
        },
        async getOptions() {
            let endpoint = `${companie}/ets/articleforminfo`
            GetData(endpoint).then((data) => {
                return data
            }).then((data) => {
                let options = []
                for (var element of data.coinTypes) {
                    options.push({ value: element.CODE, label: element.DESCRIPTION })
                }
                this.muntcode.options = options
                this.muntcode.selected = this.muntcode.options.find((x) => x.label.toLowerCase() == "euro").value
                muntcode = this.muntcode.options.find((x) => x.label.toLowerCase() == "euro").value

                options = []
                for (element of data.bankAccounts) {
                    options.push({ value: element.CODE, label: `${element.CODE} - ${element.DESCRIPTION}` })
                }
                this.rekver.options = options
                this.rekver.selected = this.rekver.options.find((x) => x.value == "700000").value
                rekver = this.rekver.options.find((x) => x.value == "700000").value

                options = []
                for (element of data.measureTypes) {
                    options.push({ value: element.CODE, label: element.SHORT_DESCRIPTION })
                }
                this.aaneh.options = options
                this.aaneh.selected = this.aaneh.options.find((x) => x.label.toLowerCase() == 'stuk').value
                aaneh = this.aaneh.options.find((x) => x.label.toLowerCase() == 'stuk').value

                options = []
                for (element of data.measureTypes) {
                    options.push({ value: element.CODE, label: element.SHORT_DESCRIPTION })
                }
                this.vereh.options = options
                this.vereh.selected = this.vereh.options.find((x) => x.label.toLowerCase() == 'stuk').value
                vereh = this.vereh.options.find((x) => x.label.toLowerCase() == 'stuk').value

                options = []
                for (element of data.BTWCodes) {
                    if (element.DESCRIPTION == null) {
                        continue
                    } else if (
                        element.DESCRIPTION.includes("EXTRA CODE") ||
                        element.DESCRIPTION.includes("TEKSTLIJN")
                    ) {
                        continue
                    } else {
                        options.push({ value: element.CODE, label: element.DESCRIPTION })
                    }
                }
                this.btwcode.options = options
                this.btwcode.selected = this.btwcode.options.find((x) => x.label.toLowerCase() == 'btw 21 %').value
                btwcode = this.btwcode.options.find((x) => x.label.toLowerCase() == 'btw 21 %').value

                options = []
                for (element of data.families) {
                    options.push({ value: element.CODE, label: element.DESCRIPTION })
                }
                this.familie.options = options
                this.familie.selected = this.familie.options.find((x) => x.label.toLowerCase() == 'niet courant materiaal').value
                familie = this.familie.options.find((x) => x.label.toLowerCase() == 'niet courant materiaal').value

                options = []
                for (element of data.subfamilies) {
                    if (element.CODE.includes('2')) {
                        options.push({ value: element.CODE, label: element.DESCRIPTION })
                    }
                }
                this.subfamilie.options = options
                this.subfamilie.selected = this.subfamilie.options.find((x) => x.label.toLowerCase() == "non stock").value
                subfamilie = this.subfamilie.options.find((x) => x.label.toLowerCase() == "non stock").value

                lengte = "0"
                breedte = "0"
                hoogte = "0"
                omrekfac = "1"
                typfac = this.typfac.options.find((x) => x.label.toLowerCase() == "vermenigvuldigingsfactor").value
            })
        },
        handleArtikelNr(value) {
            artikelNr = value
        },
        handleReflev(value) {
            reflev = value

        },
        handleOmschrijving(value) {
            omschrijving = value

        },
        handletarief(selectedOption) {
            tarief = selectedOption

        },
        handleAankoop(value) {
            aankoop = value
            if (winstpercentage.includes("33.33")) {
                verkoop = parseFloat(
                    aankoop * (1 + 1 / 3)
                ).toFixed(4)
            } else {
                verkoop = parseFloat(
                    aankoop * (1 + winstpercentage / 100)
                ).toFixed(4)
            }

            this.aankoop.placeholder = aankoop
            this.verkoop.placeholder = verkoop


        },
        handleStdKorting(value) {
            stdKorting = value

            aankoop = parseFloat(
                tarief * (1 - value / 100)
            ).toFixed(4)
            if (winstpercentage.includes("33.33")) {
                verkoop = parseFloat(
                    aankoop * (1 + 1 / 3)
                ).toFixed(4)
            } else {
                verkoop.value = parseFloat(
                    aankoop * (1 + winstpercentage / 100)
                ).toFixed(4)
            }
            this.aankoop.placeholder = aankoop
            this.verkoop.placeholder = verkoop


        },
        handleMuntcode(value) {
            muntcode = value

        },
        handleVerkoop(value) {
            verkoop = value

            winstpercentage = ((verkoop / aankoop - 1) * 100).toFixed(4)

            this.verkoop.placeholder = verkoop
            this.winstpercentage.placeholder = winstpercentage


        },
        handleWinst(value) {
            winstpercentage = value

            if (value.includes("33.33")) {
                this.verkoop.placeholder = parseFloat(
                    aankoop * (1 + 1 / 3)
                ).toFixed(4)

            } else {
                this.verkoop.placeholder = parseFloat(
                    aankoop * (1 + value / 100)
                ).toFixed(4)
            }
            verkoop = this.verkoop.placeholder

        },
        handleRekver(value) {
            rekver = value

        },
        handleAaneh(value) {
            aaneh = value
        },
        handleVereh(value) {
            vereh = value

        },
        handleBtwcode(value) {
            btwcode = value

        },
        handleOmrekfac(value) {
            omrekfac = value

        },
        handleTypfac(value) {
            typfac = value

        },
        handleMerk(value) {
            merk = value

        },
        handleFamilie(value) {
            familie = value

        },
        handleSubfamilie(value) {
            subfamilie = value
        },
        handleLengte(value) {
            lengte = value.toString()

        },
        handleBreedte(value) {
            breedte = value.toString()

        },
        handleHoogte(value) {
            hoogte = value.toString()

        },
        handleHoofdleverancier(value) {
            hoofdleverancier = value.toString()

        },
        handleMinaan(value) {
            minaan = value

        },
        createInfoObject() {
            object = {
                artikelNr: artikelNr,
                reflev: reflev,
                omschrijving: omschrijving,
                tarief: tarief,
                aankoop: aankoop,
                stdKorting: stdKorting,
                muntcode: muntcode,
                verkoop: verkoop,
                winstpercentage: winstpercentage,
                rekver: rekver,
                aaneh: aaneh,
                vereh: vereh,
                btwcode: btwcode,
                omrekfac: omrekfac,
                typfac: typfac,
                merk: merk,
                familie: familie,
                subfamilie: subfamilie,
                lengte: lengte,
                breedte: breedte,
                hoogte: hoogte,
                hoofdleverancier: hoofdleverancier,
                minaan: minaan,
            }
            const keys = Object.keys(object)
            let errors = 0
            for (const key of keys) {
                if ((object[key] == undefined || object[key] == '') && key != 'link') {
                    this.changeErrorState(key, true)
                    errors++
                } else {
                    this.changeErrorState(key, false)
                }
            }
            if (errors === 0) {
                this.emitObjectArtikel()
            }
        },
        changeErrorState(key, value) {
            switch (key) {
                case 'artikelNr':
                    this.artikelNr.error = value
                    break
                case 'reflev':
                    this.reflev.error = value
                    break
                case 'omschrijving':
                    this.omschrijving.error = value
                    break
                case 'tarief':
                    this.tarief.error = value
                    break
                case 'aankoop':
                    this.aankoop.error = value
                    break
                case 'stdKorting':
                    this.stdKorting.error = value
                    break
                case 'muntcode':
                    this.muntcode.error = value
                    break
                case 'verkoop':
                    this.verkoop.error = value
                    break
                case 'winstpercentage':
                    this.winstpercentage.error = value
                    break
                case 'rekver':
                    this.rekver.error = value
                    break
                case 'aaneh':
                    this.aaneh.error = value
                    break
                case 'vereh':
                    this.vereh.error = value
                    break
                case 'btwcode':
                    this.btwcode.error = value
                    break
                case 'omrekfac':
                    this.omrekfac.error = value
                    break
                case 'typfac':
                    this.typfac.error = value
                    break
                case 'merk':
                    this.merk.error = value
                    break
                case 'familie':
                    this.familie.error = value
                    break
                case 'subfamilie':
                    this.subfamilie.error = value
                    break
                case 'lengte':
                    this.lengte.error = value
                    break
                case 'breedte':
                    this.breedte.error = value
                    break
                case 'hoogte':
                    this.hoogte.error = value
                    break
                case 'hoofdleverancier':
                    this.hoofdleverancier.error = value
                    break
                case 'minaan':
                    this.minaan.error = value
                    break
            }
        },
        validate(object) {
            console.log(object)
            if (object.artikelNr.length > 0 || object.artikelNr != undefined) {
                this.artikelNr.error = true
                this.artikelNr.errorText = object.artikelNr.join(", ")
            }
            if (object.reflev.length > 0 && object.reflev != undefined) {
                console.log(object.reflev)
                this.reflev.error = true
                this.reflev.errorText = object.reflev.join(", ")
            }
            if (object.omschrijving.length > 0 && object.omschrijving != undefined) {
                this.omschrijving.error = true
                this.omschrijving.errorText = object.omschrijving.join(", ")
            }
            if (object.tarief.length > 0 && object.tarief != undefined) {
                this.tarief.error = true
                this.tarief.errorText = object.tarief.join(", ")
            }
            if (object.aankoop.length > 0 && object.aankoop != undefined) {
                this.aankoop.error = true
                this.aankoop.errorText = object.aankoop.join(", ")
            }
            if (object.aaneh.length > 0 && object.aaneh != undefined) {
                this.aaneh.error = true
                this.aaneh.errorText = object.aaneh.join(", ")
            }
            if (object.minaan.length > 0 && object.minaan != undefined) {
                this.minaan.error = true
                this.minaan.errorText = object.minaan.join(", ")
            }
            if (object.merk.length > 0 && object.merk != undefined) {
                this.merk.error = true
                this.merk.errorText = object.merk.join(", ")
            }
            if (object.verkoop.length > 0 && object.verkoop != undefined) {
                this.verkoop.error = true
                this.verkoop.errorText = object.verkoop.join(", ")
            }
        }
    }
}
</script>

<style scoped>
.c-artikel_form {
    display: grid;
    grid-template-areas: "artikelnr reflevnr omschrijving"
        "tarief prijs stdkorting"
        "muntcode verkoop winst"
        "rekver akprijseh verbruikseh"
        "btwcode omrekfac typfac"
        "merk familie subfamilie"
        "lengte breedte hoogte"
        "hoofdleverancier link minaan";
    grid-template-columns: 30vw 30vw 30vw;
    grid-gap: var(--global-whitespace-lg);
    width: 100vw;
}

.c-artikelNr {
    grid-area: artikelnr
}

.c-reflev {
    grid-area: reflevnr
}

.c-omschrijving {
    grid-area: omschrijving
}

.c-tarief {
    grid-area: tarief
}

.c-aankoop {
    grid-area: prijs
}

.c-stdKorting {
    grid-area: stdkorting
}

.c-muntcode {
    grid-area: muntcode
}

.c-verkoop {
    grid-area: verkoop
}

.c-winstpercentage {
    grid-area: winst
}

.c-rekver {
    grid-area: rekver
}

.c-aaneh {
    grid-area: akprijseh
}

.c-vereh {
    grid-area: verbruikseh
}

.c-btwcode {
    grid-area: btwcode
}

.c-omrekfac {
    grid-area: omrekfac
}

.c-omrekfac {
    grid-area: omrekfac
}

.c-merk {
    grid-area: merk
}

.c-familie {
    grid-area: familie
}

.c-subfamilie {
    grid-area: subfamilie
}

.c-lengte {
    grid-area: lengte
}

.c-breedte {
    grid-area: breedte
}

.c-hoogte {
    grid-area: hoogte
}

.c-hoofdleverancier {
    grid-area: hoofdleverancier
}

.c-link {
    grid-area: link
}

.c-minaan {
    grid-area: minaan
}
</style>
<template>
    <div v-show=showTabel>
        <h1> bestelbon {{ bestelbonNr }}</h1>
        <h3 v-show="showError" class="c-label--error"> Er zijn geen artikels bij deze bestelbon!</h3>
        <div v-show="showInfo" v-for="artikel in artikels" :key="artikel.leverancier.name">
            <h3> {{ artikel.leverancier.name }}</h3>
            <table>
                <thead>
                    <tr>
                        <th>Artikel</th>
                        <th>Aantal</th>
                        <th>Omschrijving</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="artikel in artikel.leverancier.artikels" :key="artikel.artikelNummer">
                        <td>{{ artikel.artikelNummer }}</td>
                        <td>{{ artikel.aantal }}</td>
                        <td>{{ artikel.omschrijving }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        showTabel: Boolean,
        bestelbonNr: String,
        showError: Boolean,
        showInfo: Boolean,
        artikels: Array
    }
}
</script>

<style scoped>
.c-label--error {
    color: var(--global-color-error);
}
</style>

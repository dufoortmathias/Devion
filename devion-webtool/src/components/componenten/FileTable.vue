<!-- ProductTable.vue -->
<template>
  <table>
    <thead>
      <tr>
        <th class="bg-blue">Product Name</th>
        <th class="bg-blue">.dxf</th>
        <th class="bg-blue">.pdf</th>
        <th class="bg-blue">.stp</th>
        <th class="bg-blue">_FLAT.dxf</th>
        <th class="bg-blue">.stl</th>
        <th class="bg-blue">.png</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="(product, index) in filteredProducts" :key="index">
        <td
          :class="{
            'bg-red': hasMissingFiles(product.files),
            'bg-green': !hasMissingFiles(product.files)
          }"
        >
          {{ product.number }}
        </td>
        <td
          :class="{
            'bg-red': product.files.dxf === 'NOK',
            'bg-green': product.files.dxf === 'OK',
            'bg-gray': product.files.dxf === 'N/A'
          }"
        >
          {{ product.files.dxf }}
        </td>
        <td
          :class="{
            'bg-red': product.files.pdf === 'NOK',
            'bg-green': product.files.pdf === 'OK',
            'bg-gray': product.files.pdf === 'N/A'
          }"
        >
          {{ product.files.pdf }}
        </td>
        <td
          :class="{
            'bg-red': product.files.stp === 'NOK',
            'bg-green': product.files.stp === 'OK',
            'bg-gray': product.files.stp === 'N/A'
          }"
        >
          {{ product.files.stp }}
        </td>
        <td
          :class="{
            'bg-red': product.files.flatDxf === 'NOK',
            'bg-green': product.files.flatDxf === 'OK',
            'bg-gray': product.files.flatDxf === 'N/A'
          }"
        >
          {{ product.files.flatDxf }}
        </td>
        <td
          :class="{
            'bg-red': product.files.stl === 'NOK',
            'bg-green': product.files.stl === 'OK',
            'bg-gray': product.files.stl === 'N/A'
          }"
        >
          {{ product.files.stl }}
        </td>
        <td
          :class="{
            'bg-red': product.files.png === 'NOK',
            'bg-green': product.files.png === 'OK',
            'bg-gray': product.files.png === 'N/A'
          }"
        >
          {{ product.files.png }}
        </td>
      </tr>
    </tbody>
  </table>
</template>
  
  <script>
export default {
  props: {
    products: {
      type: Array,
      required: true
    },
    errorToggle: {
      type: Boolean,
      required: true,
      default: false
    }
  },
  computed: {
    filteredProducts() {
      if (this.errorToggle) {
        return this.products.filter((product) => this.hasMissingFiles(product.files))
      } else {
        return this.products
      }
    }
  },
  methods: {
    hasMissingFiles(files) {
      if (
        files.dxf === 'NOK' ||
        files.pdf === 'NOK' ||
        files.stp === 'NOK' ||
        files.stl === 'NOK' ||
        files.flatDxf === 'NOK' ||
        files.png === 'NOK'
      ) {
        return true
      }
    }
  }
}
</script>
  
  <style>
.bg-red {
  background-color: #ff0000;
}

.bg-green {
  background-color: #00ff00;
}

.bg-gray {
  background-color: #808080;
}

.bg-blue {
  background-color: #0000ff;
  color: #ffffff;
}
</style>
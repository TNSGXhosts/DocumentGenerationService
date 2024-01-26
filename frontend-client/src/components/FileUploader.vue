<template>
  <div class="center-content">
    <div>
      <input type="file" ref="fileInput" @change="handleFileChange" />
      <button @click="uploadFile">Upload</button>
    </div>

    <div v-if="files.length">
      <h2>Uploaded Files</h2>
      <button class="reload-button" @click="getFiles">Reload</button>
      <table>
        <thead>
          <tr>
            <th>File Name</th>
            <th>Status</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="file in files" :key="file.id">
            <td>{{ file.fileName }}</td>
            <td>{{ getStatusText(file.status) }}</td>
            <td>
              <button @click="downloadPdf(file.id)" :disabled="file.status !== statusEnum.Success">
                Download PDF
              </button>
              <button @click="deleteFile(file.id)">
                Delete
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

const FileStatus = {
  InQueue: 0,
  Success: 1,
  Error: 2
};

export default {
  data() {
    return {
      selectedFile: null,
      files: [],
      statusEnum: FileStatus
    };
  },
  methods: {
    handleFileChange() {
      this.selectedFile = this.$refs.fileInput.files[0];
    },
    uploadFile() {
      const formData = new FormData();
      formData.append('file', this.selectedFile);

      axios.post(process.env.VUE_APP_API_UPLOAD_URL, formData)
        .then(response => {
          console.log(response.data);
          this.getFiles();
        })
        .catch(error => {
          console.error('Error uploading file:', error);
        });
    },
    downloadPdf(fileId) {
      const downloadUrl = `${process.env.VUE_APP_API_DOWNLOAD_URL}/${fileId}`;

      const link = document.createElement('a');
      link.href = downloadUrl;
      link.target = '_blank';
      link.click();
    },
    deleteFile(fileId) {
      axios.delete(`${process.env.VUE_APP_API_DELETE_URL}/${fileId}`)
        .then(response => {
          console.log(response.data);
          this.getFiles();
        })
        .catch(error => {
          console.error('Error deleting file:', error);
        });
    },
    getStatusText(status) {
      const statusEnumValues = Object.values(this.statusEnum);
      if (statusEnumValues.includes(status)) {
          return Object.keys(this.statusEnum).find(key => this.statusEnum[key] === status);
      }
      return 'Unknown';
    },
    getFiles() {
      axios.get(process.env.VUE_APP_API_GET_FILES_URL)
        .then(response => {
          this.files = response.data;
        })
        .catch(error => {
          console.error('Error fetching files:', error);
        });
    },
  },
  mounted() {
    this.getFiles();
  },
};
</script>

<style scoped>
.center-content {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.reload-button {
  margin-bottom: 10px;
  align-self: flex-end;
}
</style>

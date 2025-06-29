
export function parseApiError(error: any): string {
  if (error.response && error.response.data) {
    const data = error.response.data;
    if (data.errors) {
      const messages = Object.values(data.errors).flat();
      return messages.join("\n");
    }
    if (data.title) {
      return data.title;
    }
  }
  return "Unknown error occurred";
}
